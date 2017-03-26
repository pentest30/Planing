using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows.Controls;
using Planing.Core.Models;
using Planing.ModelView;
using Planing.UI.Helpers;

namespace Planing.PL.Generator
{
    public static class PlaningGenerator
    {
        #region Properties
        public delegate void UpdateDg(string mesage);

        public static UpdateDg UpdateDataDg;
        private static DbModel _db;
        private static List<ClassSeance> _classSeances = new List<ClassSeance>();
        private static List<TeacherSeance> _techerSeances = new List<TeacherSeance>();
        private static int[] _firstHafls;
        private static int[] _secondeHafls;
        private static List<Lecture> _planing;
        public static string Status = "";
        private static List<Lecture> _noSolutions;
        private static readonly List<List<ClassSeance>> ListOfClaassLecture = new List<List<ClassSeance>>();
        private static List<Tc> _tcs;
        #endregion
        #region Functions
        private static List<int> GetListTechersIds(int fId, int semestre, int anneeScolaire)
        {
            using (_db = new DbModel())
            {
                var result = _db.Tcs.Include("Section").Include("Section.Specialite").
                    Where(x => x.Section.Specialite.FaculteId == fId
                               && x.Semestre == semestre
                               && x.AnneeScolaireId == anneeScolaire).OrderByDescending(x => x.Periode);
                return result.Select(x => x.TeacherId).ToList();
            }
        }

        public static IEnumerable<ClassSeance> GenerateClassSeances(int fId, int anneeScoliare, int semestre)
        {
            using (_db = new DbModel())
            {
                var classes = _db.ClassRooms.
                    Include("ClassRoomType")
                    .Where(x => x.FaculteId == fId).
                    ToList();
                return ClassSeance.GenerateSeances(classes, anneeScoliare, semestre);
            }
        }

        public static List<Lecture> GeneratingPlanings(int fid, int semestre, int anneeScolaire,
            ProgressBar progressBar)
        {
            _noSolutions = new List<Lecture>();
            var teacherIds = GetListTechersIds(fid, semestre, anneeScolaire);
            _planing = new List<Lecture>();
            _techerSeances = TeacherSeance.GenerateTeacherSeances(teacherIds, semestre, anneeScolaire);
            _classSeances = GenerateClassSeances(fid, anneeScolaire, semestre).ToList();
            progressBar.Minimum = 0;
            progressBar.Maximum = teacherIds.Count();
            var pbar = new PBar(progressBar);
            using (_db = new DbModel())
            {
                _tcs = _db.Tcs.OrderBy(x => x.ClassRoomTypeId).
                    ThenByDescending(x => x.Periode).
                    ThenBy(x => x.GroupeId).
                    Include("Section").
                    Include("Section.Specialite").
                    Include("Section.SalleClasses").
                    Include("Section.Annee").
                    Include("Groupe").
                    Include("Groupe.SalleClasses").
                    Include("ClassRoomType").
                    Include("Teacher").
                    Include("Teacher.Seances").
                    Where(x => x.Section.Specialite.FaculteId == fid &&
                               x.Semestre == semestre
                               && x.Section.AnneeScolaireId == anneeScolaire).ToList();
            }
            progressBar.Maximum = _tcs.Count;
            foreach (var tc in _tcs.ToList())
            {
                var firstOrDefault = _techerSeances.FirstOrDefault(x => x.TeacherId == tc.TeacherId);
                GenerateLecture(fid, tc, firstOrDefault, pbar);
            }
            ListOfClaassLecture.Add(_classSeances);
            //CrossOver(_noSolutions);
            return _planing;
        }

        private static void GenerateLecture(int fid, Tc tc1, TeacherSeance firstOrDefault, PBar pbar)
        {


            if (firstOrDefault != null)
            {

                var sc = firstOrDefault.Seances.OrderBy(p => p).ToList();
                var temp = new List<int>();
                temp.AddRange(sc);
                var rnd = new Random();
                var rnd2 = new Random();
                for (int i = 0; i < tc1.ScheduleWieght; i++)
                {
                    var item = 0;
                    int r1;
                    var exist = false;
                    do
                    {
                        if (temp.Count == 0) break;
                        r1 = rnd.Next(0, temp.Count - 1);
                        item = temp[r1];
                        exist = CheckIfLectureExist(item, tc1);
                        temp.Remove(temp[r1]);

                    } while (!exist);
                    if (exist)
                    {
                        AddSolution(fid, tc1, pbar, item, rnd2);
                    }
                    else
                    {
                        AddNoSolution(fid, tc1, pbar);
                        // list.Remove(tc1);
                    }
                }

            }
        }

        private static void AddNoSolution(int fid, Tc tc1, PBar pbar)
        {
            //if (tc1.Periode == 1 || tc1.Periode == 2)
            //{
            //    tc1.Periode = 0;
            //    return;
            //}
            var nos = new Lecture();
            nos.TeacherId = tc1.TeacherId;
            nos.CourseId = tc1.CourseId;
            //sc.RemoveAt(r1);
            nos.ClassRoomTypeId = tc1.ClassRoomTypeId;
            nos.SectionId = Convert.ToInt32(tc1.SectionId);
            nos.GroupeId = tc1.GroupeId;
            nos.SpecialiteId = tc1.Section.SpecialiteId;
            nos.FaculteId = fid;
            nos.AnneeId = tc1.Section.AnneeId;
            nos.Solved = false;
            if (
                !_noSolutions.Any(
                    x =>
                        x.TeacherId == nos.TeacherId && x.ClassRoomTypeId == nos.ClassRoomTypeId &&
                        x.CourseId == nos.CourseId && x.SectionId == nos.SectionId && x.GroupeId == nos.GroupeId))
                _noSolutions.Add(nos);
            pbar.IncPb();
            // _tcs.Remove(tc1);
        }

        private static void AddSolution(int fid, Tc tc1, PBar pbar, int item, Random rnd2)
        {
            Tc tc2 = tc1;
            var rest = SelectRoomLecture(item, tc2);
            var  s = AnyOrDefault(rest, se =>(0.5) );
            var r2 = rnd2.Next(0, rest.Count());
            var lecture = s;
            if (lecture != null)
            {
                _classSeances.Remove(s);
                rest.Remove(s);
                
                //sc.RemoveAt(r1);
                foreach (var teacherSeance in _techerSeances)
                {
                    if (teacherSeance.TeacherId == tc1.TeacherId)
                        teacherSeance.Seances.Remove(lecture.Seance);
                }
                var pl = new Lecture();
                pl.TeacherId = tc1.TeacherId;
                pl.CourseId = tc1.CourseId;
                pl.ClassRoomId = lecture.ClassRoomId;
                pl.ClassRoomTypeId = lecture.ClassRoomTypeId;
                pl.Seance = lecture.Seance;
                pl.SectionId = Convert.ToInt32(tc1.SectionId);
                pl.GroupeId = tc1.GroupeId;
                pl.SpecialiteId = tc1.Section.SpecialiteId;
                pl.FaculteId = fid;
                pl.AnneeId = tc1.Section.AnneeId;
                pl.Periode = tc1.Periode;
                pl.Teacher = tc1.Teacher;
                pl.Teacher.Seances.AddRange(tc1.Teacher.Seances);
                pl.Solved = true;
                _planing.Add(pl);
                _tcs.Remove(tc1);
                pbar.IncPb();
                //list.Remove(tc1);
            }
        }

        private static void GenerateLectureSpecialClassRoom(int fid, Tc tc1, TeacherSeance firstOrDefault, PBar pbar)
        {
            using (_db = new DbModel())
            {
                if (tc1.Section.SalleClasses.Count > 0)
                    foreach (var salleClass in tc1.Section.SalleClasses)
                    {
                        salleClass.ClassRoom = _db.ClassRooms.FirstOrDefault(x => x.Id == salleClass.ClassRoomId);
                    }
                else
                {
                    foreach (var salleClass in tc1.Groupe.SalleClasses)
                    {
                        salleClass.ClassRoom = _db.ClassRooms.FirstOrDefault(x => x.Id == salleClass.ClassRoomId);
                    }

                }

            }
            var salleClasse = (tc1.Section.SalleClasses.Count > 0)
                ? tc1.Section.SalleClasses.FirstOrDefault(
                    x => x.ClassRoom.ClassRoomTypeId == tc1.ClassRoomTypeId)
                : tc1.Groupe.SalleClasses.FirstOrDefault(
                    x => x.ClassRoom.ClassRoomTypeId == tc1.ClassRoomTypeId);
            if (firstOrDefault != null)
            {
                if (salleClasse != null)
                {
                    var classroomid =
                        salleClasse.ClassRoomId;
                    var sc = firstOrDefault.Seances.OrderBy(p => p).ToList();

                    var temp = new List<int>();
                    if (tc1.Periode == 1) temp.AddRange(sc.Where(CheckIfExistInFirstPeriode));
                    else if (tc1.Periode == 2) temp.AddRange(sc.Where(CheckIfExistForSecondPeriode));
                    else temp.AddRange(sc);
                    var rnd = new Random();
                    var rnd2 = new Random();
                    for (int i = 0; i < tc1.ScheduleWieght; i++)
                    {
                        var item = 0;
                        var r1 = 0;

                        var exist = false;
                        do
                        {
                            if (temp.Count == 0) break;
                            r1 = rnd.Next(0, temp.Count - 1);
                            item = temp[r1];
                            exist = CheckIfLectureExist(item, tc1, classroomid);
                            temp.Remove(temp[r1]);

                        } while (!exist);
                        if (exist)
                        {
                            var tc2 = tc1;
                            var rest = SelectRoomLecture(item, tc2, classroomid);
                            var r2 = rnd2.Next(0, rest.Count());
                            var lecture = rest[r2];
                            if (lecture != null)
                            {
                                rest.RemoveAt(r2);
                                var ics =
                                    _classSeances.FirstOrDefault(
                                        x => x.ClassRoomId == lecture.ClassRoomId && x.Seance == lecture.Seance);
                                _classSeances.Remove(ics);
                                sc.RemoveAt(r1);
                                foreach (var teacherSeance in _techerSeances)
                                {
                                    if (teacherSeance.TeacherId == tc1.TeacherId)
                                        teacherSeance.Seances.Remove(lecture.Seance);
                                }
                                var pl = new Lecture();
                                pl.TeacherId = tc1.TeacherId;
                                pl.CourseId = tc1.CourseId;
                                pl.ClassRoomId = lecture.ClassRoomId;
                                pl.ClassRoomTypeId = lecture.ClassRoomTypeId;
                                pl.Seance = lecture.Seance;
                                pl.SectionId = Convert.ToInt32(tc1.SectionId);
                                pl.GroupeId = tc1.GroupeId;
                                pl.SpecialiteId = tc1.Section.SpecialiteId;
                                pl.FaculteId = fid;
                                pl.AnneeId = tc1.Section.AnneeId;
                                pl.Solved = true;
                                //Db.Lectures.Add(pl);
                                //Db.SaveChanges();
                                _planing.Add(pl);
                                pbar.IncPb();
                                _tcs.Remove(tc1);
                                //list.Remove(tc1);
                            }
                        }
                        else
                        {
                            AddNoSolution(fid, tc1, pbar);
                        }
                    }
                }
            }
        }

      
        private static void CrossOver(List<Lecture> noSolutions)
        {
            using (_db = new DbModel())
            {

                if (noSolutions.Count > 0)
                {

                    var rnd = new Random();
                    var temp = new List<Lecture>();
                    foreach (var noSolution in noSolutions)
                    {
                        Lecture solution = noSolution;
                        var tc = new Tc();
                        tc.GroupeId =  noSolution.GroupeId;
                        tc.SectionId = noSolution.SectionId;
                        tc.Section = _db.Sections.FirstOrDefault(x => x.Id == noSolution.SectionId);
                        tc.ClassRoomTypeId = noSolution.ClassRoomTypeId;
                        var listOfSeances = _classSeances.Where(x => x.ClassRoomTypeId == noSolution.ClassRoomTypeId)
                            .Select(x => x.Seance)
                            .ToList();
                        bool count;
                        var s = 0;
                        do
                        {
                            if (listOfSeances.Count == 0) break;
                            var seance = rnd.Next(0, listOfSeances.Count());
                            s = listOfSeances[seance];
                            var f = _planing.Any(x => x.Seance == s&&x.TeacherId ==tc.TeacherId);
                            count = CheckIfLectureExist(s, tc) && !f;
                            listOfSeances.Remove(s);
                        } while (!count);
                        var ct = tc.ClassRoomTypeId;
                        var lsut = (from c in _classSeances
                            where
                                c.ClassRoomTypeId == ct && c.Seance == s
                            select c).ToList();
                        if (lsut.Count == 0)
                        {
                            solution.Solved = false;
                            _planing.Add(solution);
                            continue;
                        }
                        var r2 = rnd.Next(0, lsut.Count());
                        var item = lsut[r2];
                        solution.Seance = s;
                        solution.ClassRoomId = item.ClassRoomId;
                        temp.Add(solution);
                        solution.Solved = true;
                        _planing.Add(solution);
                        _tcs.Remove(tc);
                        _classSeances.Remove(item);
                    }
                    foreach (var lecture in temp)
                    {
                        noSolutions.Remove(lecture);
                    }
                    // Mutation();
                    // noSolutions.Clear();
                    //noSolutions.AddRange(temp);
                }
                Mutation(noSolutions);
                //_db.ClassSeances.AddRange(_classSeances);
                //_db.Lectures.AddRange(_planing);
                //_db.SaveChanges();
            }
        }

      

        private static bool CheckIfExistForSecondPeriode(int s)
        {
            _secondeHafls = new[] {4, 5, 6, 10, 11, 12, 16, 17, 18, 22, 23, 24, 25, 28, 29, 30, 34, 35, 36};
            return _secondeHafls.Any(x => x == (s));
        }

        private static bool CheckIfExistInFirstPeriode(int s)
        {
            _firstHafls = new[] {1, 2, 3, 7, 8, 9, 13, 14, 15, 19, 20, 21, 25, 26, 27, 31, 32, 33};
            return _firstHafls.Any(x => x == (s));

        }

        public static int GeneratePopulations(int fId, int semestre, int anneeScolaire, ProgressBar progressBar , UpdateStatus updateStatus)
        {
            if (UpdateDataDg != null) UpdateDataDg("Calcul heuristiques ...");

            using (_db = new DbModel())
            {
                var l = _db.Tcs.Where(x => x.AnneeScolaireId == anneeScolaire && x.Semestre == semestre);
                var h =CalculHeuristics(l, fId,semestre, anneeScolaire);
                if (h == false)
                {
                    if (UpdateDataDg != null) UpdateDataDg("Sorry heuristics show that there is no Solution");
                    updateStatus.EtatHeuristique = EtatProgression.Error;
                    return 0;

                }
                updateStatus.EtatHeuristique = EtatProgression.Ok;
            }
            if (UpdateDataDg != null) UpdateDataDg("heuristiques montrent que l'algorithme peut trouver une Solution");
            var result = new List<MultiGeneration>();
            string progress = "Generation de planing ";
            var start = DateTime.Now;
            PBar pBar = new PBar(progressBar);
            progressBar.Minimum = 0;
            progressBar.Maximum = 10;
            for (int i = 0; i < 10; i++)
            {
                
                progress = progress + "++";
                if (UpdateDataDg != null) UpdateDataDg(progress);
                _planing = new List<Lecture>();
                var item = new MultiGeneration
                {
                    CountLateTime = 0,
                    Lectures = GeneratingPlanings(fId, semestre, anneeScolaire, progressBar)
                };
                updateStatus.TempSpan = DateTime.Now - start;
                //MessageBox.Show(t.ToString(CultureInfo.InvariantCulture));
                result.Add(item);
                if (UpdateDataDg != null) UpdateDataDg("Calcul de fitness.. ");
                Fitness(item);
                updateStatus.Fitness = item.CountConflict  + item.TeacherLectures;
                pBar.IncPb();
            }
           
           
            _planing = new List<Lecture>();
            double min = result.Min(x => x.CountLateTime);

            var solution =
                result.FirstOrDefault(
                    x => x.CountConflict == 0 && x.TeacherLectures == 0 && Math.Abs(x.CountLateTime - min) <= 0);
            if (solution != null)
            {
                if (solution.CountConflict == 0 && solution.TeacherLectures == 0)
                {
                    _planing = solution.Lectures;
                    if (UpdateDataDg != null) UpdateDataDg("wait please until the program saves the founded solotion ");
                    using (_db = new DbModel())
                    {
                        var index = result.FindIndex(x => x == solution);
                        _db.Lectures.AddRange(_planing);
                        _db.ClassSeances.AddRange(ListOfClaassLecture[index]);
                        _db.SaveChanges();
                        if (UpdateDataDg != null) UpdateDataDg("Done ! ");
                    }
                }
                else
                {

                    GeneratingPlanings(fId, semestre, anneeScolaire, progressBar);
                }

            }
            else
            {

                GeneratingPlanings(fId, semestre, anneeScolaire, progressBar);
            }

            return _planing != null ? _planing.Count : 0;
        }

        private static void Fitness(MultiGeneration generation)
        {
            int qnt = 0;
            int[] hafls = {6, 12, 18, 24, 30, 36};
            foreach (var lecture in generation.Lectures)
            {
                if (hafls.Any(x => x.Equals(lecture.Seance)))
                {
                    qnt ++;
                    generation.CountLateTime = qnt;
                }

            }
            foreach (var tc in generation.Lectures.Where(x => x.GroupeId != null).GroupBy(x => x.GroupeId))
            {
                for (int i = 1; i <= 36; i++)
                {
                    var f = tc.Count(x => x.Seance == i);
                    if (f > 1) generation.CountConflict += 1; ;
                }

            }
            foreach (var tc in generation.Lectures.GroupBy(x => x.SectionId))
            {
               
                for (int i = 1; i <= 36; i++)
                {
                    var f = tc.Count(x => x.Seance == i );
                    //var l = tc.SelectRoomLecture(x => x.Seance == i);
                    var f1 = tc.Count(w => w.GroupeId != 0 && w.Seance == i);
                    //var f3 = tc.Count(x => x.Seance == i);
                    if (f1 > 0 && f > f1)
                    {
                        generation.CountConflict = generation.CountConflict + 1;
                    }
                    else if (f1 == 0 && f > 1)
                    {
                        generation.CountConflict = generation.CountConflict + 1;
                    }
                   
                }
            }
            foreach (var lecture in generation.Lectures.GroupBy(x => x.TeacherId))
            {
                for (int i = 0; i < 36; i++)
                {
                    if (lecture.Count(w => w.Seance == i) > 1) generation.TeacherLectures ++;
                }
            }
            foreach (var lecture in generation.Lectures.GroupBy(x => x.ClassRoomId))
            {
                for (int i = 1; i <= 36; i++)
                {
                    if (lecture.Count(w => w.Seance == i) > 1) generation.TeacherLectures++;
                }
            }

        }
        public static T AnyOrDefault<T>( IList<T> e, Func<T, double> weightSelector)
        {
            if (e.Count < 1)
                return default(T);
            if (e.Count == 1)
                return e[0];
            var weights = e.Select(o => Math.Max(weightSelector(o), 0)).ToArray();
            var sum = weights.Sum(d => d);

            var rnd = new Random().NextDouble();
            for (int i = 0; i < weights.Length; i++)
            {
                //Normalize weight
                var w = sum == 0
                    ? 1 / (double)e.Count
                    : weights[i] / sum;
                if (rnd < w)
                    return e[i];
                rnd -= w;
            }
            throw new Exception("Should not happen");
        }
        private static List<ClassSeance> SelectRoomLecture(int item, Tc tc1)
        {


            if (tc1.GroupeId == null || tc1.GroupeId == 0)
            {
                var q = (from c in _classSeances.OrderBy(w => w.Seance)
                    where c.Seance == item && c.ClassRoomTypeId.Equals(tc1.ClassRoomTypeId)
                          && c.Max >= tc1.Section.Nombre
                          && c.Min <= tc1.Section.Nombre
                    select c).ToList();

                return q;
            }
            var q1 = (from c in _classSeances.OrderBy(w => w.Seance)
                where c.Seance == item && c.ClassRoomTypeId.Equals(tc1.ClassRoomTypeId)
                      && c.Max >= tc1.Groupe.Nombre
                      && c.Min <= tc1.Groupe.Nombre
                select c).ToList();

            return q1;
        }

        private static List<ClassSeance> SelectRoomLecture(int item, Tc tc1, int classRoomId)
        {
            if (tc1.Groupe == null)
            {
                var q = (from c in _classSeances.OrderBy(w => w.Seance)
                    where c.Seance == item && c.ClassRoomId.Equals(classRoomId)
                          && c.Max >= tc1.Section.Nombre
                          && c.Min <= tc1.Section.Nombre
                    select c).ToList();

                return q;
            }
            var q1 = (from c in _classSeances.OrderBy(w => w.Seance)
                where c.Seance == item && c.ClassRoomId.Equals(classRoomId)
                      && c.Max >= tc1.Groupe.Nombre
                      && c.Min <= tc1.Groupe.Nombre
                select c).ToList();

            return q1;
        }

        private static void Mutation(List<Lecture> noSolutions)
        {
            if (noSolutions.Count == 0)
            {
                ListOfClaassLecture.Add(_classSeances);
                return;
            }
            var rnd = new Random();
            var temp = new List<ClassSeance>();
            foreach (var tc in _planing.GroupBy(x => x.SectionId))
            {
                for (int i = 1; i <= 36; i++)
                {
                    var f = tc.Count(x => x.Seance == i);

                    var f1 = tc.Count(w => w.GroupeId != null && w.Seance == i);
                    if (f1 != 0 && f > f1)
                    {
                        int i1 = i;
                        var l = tc.Where(x => x.Seance == i1 && x.GroupeId != null);
                        foreach (var lecture in l)
                        {
                            if (_classSeances.Count > 0)
                            {
                                Lecture lecture1 = lecture;
                                var remains =
                                    _classSeances.Where(
                                        x =>
                                            x.Seance != lecture1.Seance && x.ClassRoomTypeId == lecture1.ClassRoomTypeId);
                                var classSeances = remains as ClassSeance[] ?? remains.ToArray();
                                var r2 = rnd.Next(0, classSeances.Count()-1);
                                var item = classSeances[r2];
                                if (item != null)
                                {
                                    var clas = new ClassSeance
                                    {
                                        Seance = lecture1.Seance,
                                        ClassRoomId = Convert.ToInt32(lecture.ClassRoomId),
                                        ClassRoomTypeId = lecture.ClassRoomTypeId
                                    };
                                    temp.Add(clas);
                                    _classSeances.Remove(item);
                                    lecture.Seance = item.Seance;
                                    lecture.ClassRoomId = item.ClassRoomId;
                                }
                            }
                            else
                            {
                                noSolutions.Add(lecture);
                            }

                        }
                    }
                }
            }
            foreach (var classSeance in temp)
            {
                _classSeances.Remove(classSeance);
            }
            ListOfClaassLecture.Add(_classSeances);
            temp.Clear();
        }

        private static bool CalculHeuristics(IEnumerable<Tc> tcs, int fId, int semestre, int annee)
        {
            var b = false;
            var mins = GenerateClassSeances(fId, annee, semestre);
            var classSeances = mins as ClassSeance[] ?? mins.ToArray();
            using (_db = new DbModel())
            {

                foreach (var tc in tcs.GroupBy(w => w.ClassRoomTypeId))
                {

                    var item = tc.FirstOrDefault();
                    var sum = tc.Sum(w => w.ScheduleWieght);
                    b = sum <=
                        classSeances.Count(w => item != null && w.ClassRoomTypeId == item.ClassRoomTypeId);
                    if (!b) return false;
                }
                return b;
            }

        }

        private static bool CheckIfLectureExist(int item, Tc tc, int classRoomId)
        {
            if (tc.GroupeId == null || tc.GroupeId == 0)
            {
                if (_planing.Any(lecture => lecture.SectionId == tc.SectionId && lecture.Seance == item))
                {
                    return false;
                }
                var q = (from c in _classSeances.OrderBy(w => w.Seance)
                         where c.Seance == item && c.ClassRoomId.Equals(classRoomId)
                               && c.Max >= tc.Section.Nombre
                               && c.Min <= tc.Section.Nombre
                         select c).Any();

                return q;
            }
            if (tc.GroupeId != null)
            {
                if (
                    _planing.Any(
                        lecture =>
                            (lecture.SectionId == tc.SectionId && lecture.GroupeId == null) && lecture.Seance == item))
                {
                    return false;
                }
                if (_planing.Any(lecture => (lecture.GroupeId == tc.GroupeId) && lecture.Seance == item))
                {
                    return false;
                }

            }
            
           
            var q1 = (from c in _classSeances.OrderBy(w => w.Seance)
                where c.Seance == item && c.ClassRoomId.Equals(classRoomId)
                      //&& c.Max >= tc.Groupe.Nombre
                      //&& c.Min <= tc.Groupe.Nombre
                select c).Any();

            return q1;
        }

        private static bool CheckIfLectureExist(int item, Tc tc)
        {
            if (tc.GroupeId == null || tc.GroupeId == 0)
            {
                if (_planing.Any(lecture => lecture.SectionId == tc.SectionId && lecture.Seance == item))
                {
                    return false;
                }
                var q = (from c in _classSeances.OrderBy(w => w.Seance)
                    where c.Seance == item && c.ClassRoomTypeId.Equals(tc.ClassRoomTypeId)
                          && c.Max >= tc.Section.Nombre
                          && c.Min <= tc.Section.Nombre
                    select c).Any();

                return q;
            }
            if (tc.GroupeId != null)
            {
                if (
                    _planing.Any(
                        lecture =>
                            (lecture.SectionId == tc.SectionId && lecture.GroupeId == null) && lecture.Seance == item))
                {
                    return false;
                }
                if (_planing.Any(lecture => (lecture.GroupeId == tc.GroupeId) && lecture.Seance == item))
                {
                    return false;
                }

            }
            var q1 = (from c in _classSeances.OrderBy(w => w.Seance)
                where c.Seance == item && c.ClassRoomTypeId.Equals(tc.ClassRoomTypeId)
                      //&& c.Max >= tc.Groupe.Nombre
                      //&& c.Min <= tc.Groupe.Nombre
                select c).Any();

            return q1;
        }





        #endregion

    }
}
