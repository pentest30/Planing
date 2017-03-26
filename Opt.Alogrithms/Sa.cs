using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Metaheuristics;
using Planing.Core.DbImport;
using Planing.Core.Models;

namespace Opt.Alogrithms
{
    public class Tavu
    {
        public int Period { get; set; }
        public int? Id { get; set; }
    }
    public class Sa:IMetaheuristic
    {
        public readonly int[] Periods = new int[36];
        public  readonly List<ClassRoom> Rooms;
        List<Tavu> _tabuSction = new List<Tavu>();
        List<Tavu> _tabuGrp =  new List<Tavu>();
        List<Tavu>tabuRoos = new List<Tavu>();
        public Sa()
        {
            Liste = PopulateTcs();
            Rooms = PopulateRooms();
            Solutions =  new List<Lecture>();
            for (int i = 1; i < 37; i++)
            {
                Periods[i - 1] = i;
            }
           
        }
        public List<Tc> Liste { get; set; }
        public List<Lecture> Solutions { get; set; }

        private List<Tc> PopulateTcs()
        {
            var chemin = @"C:\Users\pen\Planing\Planing\Tcs.xlsx";
            var table = DbAcceess.DataExcel(chemin, 0);
            return DbToObject.GetListFromTable<Tc>(table);
        }
        private List<ClassRoom> PopulateRooms()
        {
            var chemin = @"C:\Users\pen\Planing\Planing\Rooms.xlsx";
            var table = DbAcceess.DataExcel(chemin, 0);
            return DbToObject.GetListFromTable<ClassRoom>(table);
        }

        public List<Lecture> Run()
        {
            return RadomSolution();

        } 
        private List<Lecture> RadomSolution()
        {
            var result = new List<Lecture>();

            if (Liste.Count==0)return result;
           // var ls = Liste.GroupBy(x => x.SectionId);
         
           _tabuGrp =  new List<Tavu>();
            _tabuSction= new List<Tavu>();
            foreach (var tc in Liste.OrderBy(x=>x.SectionId))
            {
              
                var i = 0;
                var p=0; 
          
               
                var ps = new List<int>();
                for (int j = 0; j < 36; j++)
                {
                    ps.Add( j + 1);
                }
                Parallel.For(0, tc.ScheduleWieght, (k) =>
                {

                });
                for (int j = 0; j < tc.ScheduleWieght; j++)
                {
                    bool exist;
                    
                    Lecture pl;
                    ClassRoom room;
                    int ins = 0;
                   // ClassPeriod firstOrDefault = rr.FirstOrDefault(x => x.SectionId == tc.SectionId);
                    do
                    {
                        bool exist2;
                        
                        //  p = rnd.Next(0, ps.Count - 1);
                        do
                        {
                            var rnd2 = new Random();
                            var rnd = new Random();
                            p = rnd.Next(1, 36);
                            ins = rnd2.Next(0, Rooms.Count(x => x.ClassRoomTypeId == tc.ClassRoomTypeId) - 1);
                            room = Rooms[ins];
                            exist2 = tabuRoos.Any(x => x.Period == p && x.Id == room.Id);
                            
                          

                           
                        } while (exist2);
                       

                        exist = tc.GroupeId == 0 ? _tabuSction.Any(x => x.Period == p && x.Id == tc.SectionId) :
                           (_tabuGrp.Any(x => x.Period == p && x.Id == tc.GroupeId) || _tabuSction.Any(x => x.Period == p && x.Id == tc.SectionId));
                        
                        
                    } while (exist);
                    pl = CreateSolution(tc, Rooms[ins], p);
                    if (pl.GroupeId > 0)
                    {
                        //_tabuSction.Add(new Tavu { Id = pl.SectionId, Period = p });
                        _tabuGrp.Add(new Tavu { Id = pl.GroupeId, Period = p });


                    }
                    if (pl.GroupeId == 0)
                    {
                        _tabuSction.Add(new Tavu { Id = pl.SectionId, Period = p });
                        //ps.Remove(p);
                    }
                    tabuRoos.Add(new Tavu { Id = pl.ClassRoomId, Period = p });
               //     if (firstOrDefault != null && firstOrDefault.Periods.Count > 0) firstOrDefault.Periods.Remove(firstOrDefault.Periods[i]);
                    result.Add(pl);
                    Debug.WriteLine(result.Count);
                }

            }
            return result;
        }

        private static bool CheckIfLectureExist(int item, Lecture lec, List<Lecture> solution)
        {
            int final = 0;
            var f = solution.Count(x => x.SectionId == lec.SectionId && x.Seance == item);
            //var l = tc.SelectRoomLecture(x => x.Seance == i);
            var f1 = (from p in solution
                      where (p.GroupeId == lec.GroupeId) && (p.Seance == item)
                      select p).Count();
            var f3 = solution.Count(x => (x.TeacherId == lec.TeacherId) && (x.Seance == item));
            var f4 = solution.Count(x => (x.ClassRoomId == lec.ClassRoomId) && (x.Seance == item));
            if (f1 > 0 && f > f1)
            {
                final++;
            }
            if (f1 == 0 && f > 1)
            {
                final++;
            }
            if (f3 > 0) final++;
            if (f4 > 0) final++;

            return final > 0;
        }

        private Lecture CreateSolution(Tc tc, ClassRoom classRoom, int period)
        {
            var pl = new Lecture();
            pl.TeacherId = tc.TeacherId;
            pl.CourseId = tc.CourseId;
            pl.ClassRoomId = classRoom.Id;
            pl.ClassRoomTypeId = (int)tc.ClassRoomTypeId;
            pl.Seance = period;
            pl.SectionId = Convert.ToInt32(tc.SectionId);
            pl.GroupeId = tc.GroupeId;
            pl.FaculteId = 1;

            pl.Solved = true;
          
            return pl;
        }

        private int CalculFitness(List<Lecture> solution)
        {
            int final = 0;
            foreach (var tc in solution.GroupBy(x => x.SectionId))
            {
                for (int i = 1; i <= 36; i++)
                {
                    var f = tc.Count(x => x.Seance == i);
                    //var l = tc.SelectRoomLecture(x => x.Seance == i);
                    var f1 = tc.Count(w => w.GroupeId != null && w.Seance == i);
                    //var f3 = tc.Count(x => x.Seance == i);
                    if (f1 > 0 && f > f1)
                    {
                       final ++;
                    }
                    else if (f1 == 0 && f > 1)
                    {
                        final++;
                    }
                }
            }
            foreach (var lecture in solution.GroupBy(x => x.TeacherId))
            {
                for (int i = 0; i < 36; i++)
                {
                    if (lecture.Count(w => w.Seance == i) > 1) final++;
                }
            }
            return final;
        }

        public void Start(string inputFile, string outputFile, int timeLimit)
        {
            throw new NotImplementedException();
        }

        public string Name { get; private set; }
        public MetaheuristicType Type { get; private set; }
        public ProblemType Problem { get; private set; }
        public string[] Team { get; private set; }
    }

    class ClassPeriod
    {
        public ClassPeriod()
        {
            Periods =  new List<int>();
        }
        public int SectionId { get; set; }
        public List<int> Periods { get; set; }
    }
}
