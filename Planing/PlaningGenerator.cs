using System;using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Planing.Models;
using Planing.UI.Helpers;
using ProgressBar = System.Windows.Controls.ProgressBar;

namespace Planing
{
    public class PlaningGenerator
    {
        public delegate void UpdateDg(string mesage);
        public static UpdateDg UpdateDataDg;
        private static DbModel _db;
        private static List<ClassSeance> _classSeances = new List<ClassSeance>();
        private static int[] _firstHafls;
        private static int[] _secondeHafls;
        private static List<Lecture> _planing;
        public static string  Stat = "";
        private static List<Lecture> _noSolutions;
        private static readonly List<List<ClassSeance>> ListOfClaassLecture = new List<List<ClassSeance>>(); 
      private static List<int> GetListTechersIds(int facId)
        {
            using (_db = new DbModel())
            {
                var result = _db.Teachers.OrderByDescending(w => w.Seances.Count).Where(x => x.FaculteId == facId);
                return result.Select(x => x.Id).ToList();
            }
        }

        private static IEnumerable<ClassSeance> GenerateClassSeances(int facId, int anneeScoliare, int semestre)
        {
            using (_db = new DbModel())
            {
                var classes = _db.ClassRooms.
                    Include("ClassRoomType")
                    .Where(x => x.FaculteId == facId).
                    ToList();
                return ClassSeance.GenerateSeances(classes ,anneeScoliare,semestre);
            }
        }

        public static List<Lecture> GenerateLectures(int facid, int semestre, int anneeScolaire, ProgressBar progressBar )
        {
            _noSolutions = new List<Lecture>();
            var teacherIds = GetListTechersIds(facid);
           _classSeances = GenerateClassSeances(facid ,anneeScolaire,semestre).ToList();
            _planing = new List<Lecture>();
            var techerSeances = TeacherSeance.GenerateTeacherSeances(teacherIds, semestre,anneeScolaire);
            progressBar.Minimum = 0;
            progressBar.Maximum = teacherIds.Count();
            var pbar = new PBar(progressBar);
            // int e = _classSeances.Count;
            foreach (var techarId in teacherIds)
            {
                List<Tc> tcs;
                using (_db = new DbModel())
                {
                    tcs = _db.Tcs.OrderBy(x=>x.ClassRoomTypeId).ThenByDescending(x=>x.Periode).
                        Include("Section").
                        Include("Section.Specialite").
                        Include("Section.Annee").
                        Include("Groupe").
                        Include("ClassRoomType").
                        Where(x => x.TeacherId == techarId
                                   && x.Semestre == semestre
                                   && x.Section.AnneeScolaireId == anneeScolaire).ToList();
                }
                if (tcs.Count == 0)
                {
                    pbar.IncPb();
                    continue;
                }
                var firstOrDefault = techerSeances.FirstOrDefault(x => x.TeacherId == techarId);

                if (firstOrDefault != null)
                {
                    var sc = firstOrDefault.Seances.OrderBy(p => p).ToList();
                    if (sc.Count == 0 || _classSeances.Count == 0)
                    {
                        pbar.IncPb();
                        continue;
                    } 
                    var temp2 = new List<int>();
                        temp2.AddRange(sc); 
                    var temp = new List<int>();

                    foreach (var tc in tcs.OrderBy(x => x.ClassRoomTypeId).ThenByDescending(x => x.Periode))
                    {
                        var rnd2 = new Random();
                        temp.AddRange(sc);
                           
                        for (var i = 0; i < tc.ScheduleWieght; i++)
                        {

                            var b = false;
                            var item = 0;
                            var r1 = 0;
                            var rnd = new Random();
                            var exist=false;
                            do
                            {
                               
                                if (tc.Periode == 1 || tc.Periode == 2)
                                {
                                   
                                    //list temp pour evité l'infinité de la boucle.
                                  
                                    do
                                    {
                                        if (temp.Count == 0)
                                        {
                                            //r1 = rnd.Next(0, sc.Count - 1);
                                            tc.Periode = 0;
                                            break;
                                        }
                                        r1 = rnd.Next(0, temp.Count - 1);
                                        exist = CheckIfExist(sc[r1], tc);
                                        temp.Remove(temp[r1]);
                                       // if (temp.Count != 0) continue;
                                       
                                    } while (!exist);
                                    r1 = rnd.Next(0, temp2.Count - 1);
                                    item = temp2[r1];
                                    b = Any(item, tc);
                                    // if (e==0)break;
                                    temp2.Remove(item);

                                }
                                else
                                {
                                    if (temp2.Count == 0 && !exist)
                                    {

                                        break;
                                    }
                               
                                    r1 = rnd.Next(0, temp2.Count -1); 
                                    item = temp2[r1];
                                    b = Any(item, tc);
                                // if (e==0)break;
                                    temp2.Remove(item);
                                }
                                if(temp2.Count==0) break;
                               
                               

                            } while (!b);
                            if (temp2.Count == 0 &&b==false)
                            {
                                var nos = new Lecture();
                                nos.TeacherId = tc.TeacherId;
                                nos.CourseId = tc.CourseId;
                                sc.RemoveAt(r1);
                                nos.ClassRoomTypeId = tc.ClassRoomTypeId;
                                //nos.ClassRoomId = lecture.ClassRoomId;
                                //nos.Seance = lecture.Seance;
                                nos.SectionId = Convert.ToInt32(tc.SectionId);
                                nos.GroupeId = tc.GroupeId;
                                nos.SpecialiteId = tc.Section.SpecialiteId;
                                nos.FaculteId = facid;
                                nos.AnneeId = tc.Section.AnneeId;
                                nos.Solved = false;
                                _noSolutions.Add(nos);
                                pbar.IncPb();
                                continue;

                            }
                            Tc tc1 = tc;
                            var rest = Where(item, tc1);
                            var r2 = rnd2.Next(0, rest.Count());
                            var lecture = rest[r2];
                            if (lecture != null)
                            {
                                rest.RemoveAt(r2);
                                var ics =
                                    _classSeances.FirstOrDefault(
                                        x => x.ClassRoomId == lecture.ClassRoomId && x.Seance == lecture.Seance);
                                _classSeances.Remove(ics);
                                sc.RemoveAt( r1);
                                var pl = new Lecture();
                                pl.TeacherId = tc.TeacherId;
                                pl.CourseId = tc.CourseId;
                                pl.ClassRoomId = lecture.ClassRoomId;
                                pl.ClassRoomTypeId = lecture.ClassRoomTypeId;
                                pl.Seance = lecture.Seance;
                                pl.SectionId = Convert.ToInt32(tc.SectionId);
                                pl.GroupeId = tc.GroupeId;
                                pl.SpecialiteId = tc.Section.SpecialiteId;
                                pl.FaculteId = facid;
                                pl.AnneeId = tc.Section.AnneeId;
                                pl.Solved = true;
                                //Db.Lectures.Add(pl);
                                //Db.SaveChanges();
                                _planing.Add(pl);
                                pbar.IncPb();
                            }
                        }

                    }
                }
            }

            CrossOver(_noSolutions);
           
            return _planing;
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
                        tc.GroupeId = noSolution.GroupeId;
                        tc.SectionId = noSolution.SectionId;
                        tc.Section = _db.Sections.FirstOrDefault(x => x.Id == noSolution.SectionId);
                        tc.ClassRoomTypeId = noSolution.ClassRoomTypeId;
                        var listOfSeances =
                            _classSeances.Where(x => x.ClassRoomTypeId == noSolution.ClassRoomTypeId)
                                .Select(x => x.Seance)
                                .ToList();
                        bool count;
                        var s = 0;
                        do
                        {
                            if (listOfSeances.Count == 0) break;
                            var seance = rnd.Next(0, listOfSeances.Count());
                            s = listOfSeances[seance];
                            var f = _planing.Any(x => x.TeacherId == noSolution.TeacherId && x.Seance == s);
                            count = Any(s, tc)&& !f;
                            listOfSeances.Remove(s);
                        } while (!count);
                        var ct = tc.ClassRoomTypeId;
                        var lsut = (from c in _classSeances
                            where
                                c.ClassRoomTypeId == ct && c.Seance == s
                            select c).ToList();
                        var r2 = rnd.Next(0, lsut.Count());
                        var item = lsut[r2];
                        solution.Seance = s;
                        solution.ClassRoomId = item.ClassRoomId;
                        temp.Add(solution);
                        solution.Solved = true;
                        _planing.Add(solution);
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

        private static bool  CheckIfExist(int s ,Tc item)
        {
            _firstHafls = new[] { 1, 2, 3,4, 7, 8, 9,10, 13, 14, 15,16, 19, 20, 21,22, 25, 26, 27,28, 31, 32, 33,34 };
            _secondeHafls = new[] {  5, 6, 11, 12, 17, 18, 23, 24, 29, 30, 35, 36 };
            if (item.Periode == 1) return _firstHafls.Any(x => x==(s));
            return _secondeHafls.Any(x => x==(s));
        }

        public static int GeneratePopulations(int facid, int semestre, int anneeScolaire, ProgressBar progressBar)
        {
            
            using (_db = new DbModel())
            {
                var h = CalculHeuristics(_db.Tcs.Where(x => x.AnneeScolaireId == anneeScolaire && x.Semestre == semestre).ToList(), facid, semestre,anneeScolaire);
                if (h == false)
                {
                      if(UpdateDataDg!= null) UpdateDataDg(  "Sorry heuristics show that there is no solution");
                    return 0;
                }
            }
            if (UpdateDataDg != null) UpdateDataDg("heuristiques montrent que l'algorithme peut trouver une solution");
            var result = new List<MultiGeneration>();
            string progress = "Generation de planing ";
           
            for (int i = 0; i < 10; i++)
            {
                //noSolutions = new List<Lecture>();
                progress = progress + "++";
                if (UpdateDataDg != null) UpdateDataDg(progress);
                _planing = new List<Lecture>();
                var item = new MultiGeneration
                {
                    CountLateTime = 0,
                    Lectures = GenerateLectures(facid, semestre, anneeScolaire, progressBar)
                };
                result.Add(item);
            }
            foreach (var multiGeneration in result)
            {
                Fitness(multiGeneration);
            }
            _planing = new List<Lecture>();
            var solution = result.
                OrderBy(x => x.CountConflict).
                ThenBy(x => x.CountLateTime).FirstOrDefault();
            if (solution != null)
            {
                if (solution.CountConflict == 0)
                {
                    _planing = solution.Lectures;
                      if(UpdateDataDg!= null) UpdateDataDg(  "wait please until the program saves the founded solotion ");
                    using (_db = new DbModel())
                    {
                        var index =
                            result.FindIndex(
                                x =>x== solution);

                        _db.Lectures.AddRange(_planing);
                        _db.ClassSeances.AddRange(ListOfClaassLecture[index]);
                        _db.SaveChanges();
                        if (UpdateDataDg != null) UpdateDataDg("Done ! ");
                    }
                }
                else
                {
                   
                    GeneratePopulations(facid, semestre, anneeScolaire, progressBar);
                }
                
            }
           
            return _planing != null ? _planing.Count : 0;
        }
        private static void Fitness(MultiGeneration generation)
        {
            int qnt = 0;
            int[] hafls = {  6,  12,  18,  24,  30,  36 };
            foreach (var lecture in generation.Lectures)
            {
                if (hafls.Any(x => x.Equals(lecture.Seance)))
                {
                    qnt ++;
                    generation.CountLateTime = qnt;
                }

            }
            foreach (var tc in generation.Lectures.GroupBy(x => x.SectionId))
            {
                for (int i = 1; i <= 36; i++)
                {
                    var f = tc.Count(x => x.Seance == i);
                    //var l = tc.Where(x => x.Seance == i);
                    var f1 = tc.Count(w => w.GroupeId != null && w.Seance == i);
                    //var f3 = tc.Count(x => x.Seance == i);
                    if (f1 > 0 && f > f1)
                    {
                        generation.CountConflict = generation.CountConflict + 1;
                    }
                    else if(f1==0 && f1>1)
                    {
                        generation.CountConflict = generation.CountConflict + 1;
                    }
                }
            }

        }
        private static List<ClassSeance> Where(int item, Tc tc1)
        {
            if (tc1.Groupe == null)
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
                                        x => x.Seance != lecture1.Seance && x.ClassRoomTypeId == lecture1.ClassRoomTypeId);
                                var classSeances = remains as ClassSeance[] ?? remains.ToArray();
                                var r2 = rnd.Next(0, classSeances.Count());
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
        private static bool CalculHeuristics(IEnumerable<Tc> tcs , int facid, int semestre, int annee)
        {
            var b = false;
            var mins = GenerateClassSeances(facid, annee, semestre);
            var classSeances = mins as ClassSeance[] ?? mins.ToArray();
            using (_db = new DbModel())
            {

                foreach (var tc in tcs.GroupBy(w => w.ClassRoomTypeId))
                {
                    
                    var item = tc.FirstOrDefault();
                    var sum = tc.Sum(w => w.ScheduleWieght);
                    b = sum<=
                        classSeances.Count(w => item != null && w.ClassRoomTypeId == item.ClassRoomTypeId);
                    if (!b) return false;
                }
                return b;
            }
           
        }
        private static bool Any(int item, Tc tc)
        {
            if (tc.Groupe == null)
            {
                var qs = (from t in _planing
                           where
                           (t.SectionId == tc.SectionId) && t.Seance == item 
                           select t).Any();
                if (qs) return false;
                var q = (from c in _classSeances.OrderBy(w => w.Seance)
                         where c.Seance == item && c.ClassRoomTypeId.Equals(tc.ClassRoomTypeId)
                               && c.Max >= tc.Section.Nombre
                               && c.Min <= tc.Section.Nombre
                         select c).Any();

                return q;
            }
            var qs3=(from t in _planing
                     where (t.GroupeId == null && t.SectionId == tc.SectionId)&&t.Seance == item
                       select t).Any();
            if (qs3 ) return false;
            var qs2 = (from t in _planing
                       where (tc.GroupeId == t.GroupeId&&t.Seance == item   )
                       select t).FirstOrDefault();
            
           // bool chv = qs2 == null;
            if (qs2 != null) return false;
            var q1 = (from c in _classSeances.OrderBy(w => w.Seance)
                      where c.Seance == item && c.ClassRoomTypeId.Equals(tc.ClassRoomTypeId)
                            && c.Max >= tc.Groupe.Nombre
                            && c.Min <= tc.Groupe.Nombre
                      select c).Any();

            return q1 ;
        }
        //private static bool Exist(List<Lecture> temp, Tc tc, Random rnd, bool exist, List<int> sc)
        //{
        //    int r1;
        //    do
        //    {
        //        if (temp.Count == 0)
        //        {
        //            //r1 = rnd.Next(0, sc.Count - 1);
        //            tc.Periode = 0;
        //            break;
        //        }
        //        r1 = rnd.Next(0, temp.Count - 1);
        //        exist = CheckIfExist(sc[r1], tc);
        //        temp.Remove(temp[r1]);
        //        // if (temp.Count != 0) continue;
        //    } while (!exist);
        //    return exist;
        //}
    }
}
