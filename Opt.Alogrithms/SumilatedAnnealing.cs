using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Planing.Core.Models;

namespace Opt.Alogrithms
{
    public class SumilatedAnnealing
    {
        public delegate void UpdateDg();
        public UpdateDg UpdateDataDg;
        //private List<Tavu> tabu ;
        private readonly List<Lecture> _initial;
        private double _temperature;
        readonly int[] _fIrstHafls = { 1, 2, 3, 4, 5, 7, 8, 9, 10, 11, 13, 14, 15, 16, 17, 19, 20, 21, 22, 23, 25, 26, 27, 28, 29, 31, 32, 33, 34, 35 };
         int [] _dayPereiods = new[] { 1, 2, 3, 7, 8, 9, 13, 14, 15, 19, 20, 21, 25, 26, 27, 31, 32, 33 };
        public ObservableCollection<Lecture> Solution { get; set; }
        Dictionary<int , List<Lecture>> _bestSolution = new Dictionary<int, List<Lecture>>();
        public int Fitness { get; set; }
     
        public bool Stop { get; set; }
       
        //public bool _stop;
      //  public Sa Sa { get; set; }

        public SumilatedAnnealing(List<Lecture> initial )
        {
           
          //  Sa = new Sa();
            Stop = false;
            Solution = new ObservableCollection<Lecture>();
            _initial = initial;
            
        }

        public ObservableCollection<Lecture> Run(double temperature)
        {
            _temperature = temperature;
         
            double coolingRate = 0.003;
         
            //_stop = false;


            foreach (var lecture in _initial)
            {
                Solution.Add(lecture);
              
            }
            if (UpdateDataDg != null) UpdateDataDg();
            int oldValue;
            int newValue;

           
      
            oldValue = CalculFitness(Solution)+CalculSoftFitness(Solution);
            var clone = new ObservableCollection<Lecture>();
            foreach (var lecture in Solution)
            {
                clone.Add(lecture);
            }
            while (_temperature > 1 ||Stop )
            {
                var newSolution=Perturbation(clone);
                int s = CalculFitness(newSolution) ;
                newValue = s + CalculSoftFitness(Solution);
                var q = P(newValue, oldValue);
                if (newValue < oldValue||q)
                {
                    clone = new ObservableCollection<Lecture>();

                    foreach (var lecture in Solution)
                    {
                        clone.Add(lecture);
                    }
                    oldValue = CalculFitness(clone) + CalculSoftFitness(clone);
                   //// _temperature /= Convert.ToDouble(1 - coolingRate);
                    continue;
                }
                Solution = new ObservableCollection<Lecture>();
                foreach (var lecture in newSolution)
                {
                    Solution.Add(lecture);
                }
                Fitness = newValue;
                if (_bestSolution.Count == 0&&s==0)
                {
                    _bestSolution.Add( newValue, Solution.AsEnumerable().ToList());
                }
                else
                {
                    if (_bestSolution.FirstOrDefault().Key < (newValue)&&s==0)
                    {
                        _bestSolution = new Dictionary<int, List<Lecture>>();
                        _bestSolution.Add(newValue-s, Solution.AsEnumerable().ToList());
                    }
                }
                if(_bestSolution.FirstOrDefault().Value!=null)Solution = new ObservableCollection<Lecture>(_bestSolution.FirstOrDefault().Value.ToList());
                if (UpdateDataDg != null) UpdateDataDg();
                oldValue = newValue;
                _temperature *= Convert.ToDouble(1 - coolingRate);
                Debug.WriteLine(s); 
                Debug.WriteLine(_temperature);
                Debug.WriteLine(newValue);
            }
           
      
            return Solution;
        }

        private int CalculSoftFitness(ObservableCollection<Lecture> solution)
        {
            int final = 0;
            foreach (var lecture in solution)
            {
                if (CheckIfExistInFirstPeriode(lecture.Seance)) final += 550;
                if (CheckIfExistForSecondPeriode(lecture.Seance)) final -= 350;
            }
            foreach (var t   in solution.GroupBy(x=>x.TeacherId))
            {
                foreach (var lecture in t)
                {
                    if (lecture.Teacher.Seances.Count > 0)
                    {
                        if (lecture.Teacher.Seances.Any(x => x.Number == lecture.Seance)) final -= 300;
                        else
                        {
                            final += 300;
                        }
                    }
                }
            }
            final = solution.GroupBy(x => x.SectionId).Aggregate(final, (current, lecture) => current - lecture.Sum(x => x.Seance));
            foreach (var lecture in solution)
            {
                if (lecture.Periode == 1 && CheckIfExistInFirstPeriode2(lecture.Seance)) final += 300;
                if (lecture.Periode == 2 && CheckIfExistForSecondPeriode2(lecture.Seance)) final += 300;
                if (lecture.Periode == 2 && CheckIfExistInFirstPeriode2(lecture.Seance)) final -= 500;
                if (lecture.Periode == 1 && CheckIfExistForSecondPeriode2(lecture.Seance)) final -= 500;
            }
            return final;
        }
        private static bool CheckIfExistForSecondPeriode(int s)
        {
            var secondeHafls = new[] {6,   12,   18,  24,  30, 36 };
            return secondeHafls.Any(x => x == (s));
        }
       


        private static bool CheckIfExistInFirstPeriode(int s)
        {
            var firstHafls = new[] { 1, 2, 3, 4, 5, 7, 8, 9, 10, 11, 13, 14, 15, 16, 17, 19, 20, 21, 22, 23, 25, 26, 27,28,29, 31, 32, 33, 34,35 };
            return firstHafls.Any(x => x == (s));

        }
        private static bool CheckIfExistForSecondPeriode2(int s)
        {
            var secondeHafls = new[] { 4, 5, 6, 10, 11, 12, 16, 17, 18, 22, 23, 24, 25, 28, 29, 30, 34, 35, 36 };
            return secondeHafls.Any(x => x == (s));
        }

        private static bool CheckIfExistInFirstPeriode2(int s)
        {
            var firstHafls = new[] { 1, 2, 3, 7, 8, 9, 13, 14, 15, 19, 20, 21, 25, 26, 27, 31, 32, 33 };
            return firstHafls.Any(x => x == (s));

        }
        private ObservableCollection<Lecture> Perturbation(ObservableCollection<Lecture> solution)
        {
            bool f ;
            int p;
            int index;
            List<int >peridos = new List<int>();
            for (int i = 1; i < 37; i++)
            {
                peridos.Add(i);
            }
            Lecture selected;
            var rnd2 = new Random();
            do
            {
                //bool exist2;
               
                var s0 = CalculFitness(solution);
                var f0 = s0;
                index = rnd2.Next(0, solution.Count() - 1);
                selected = solution[index];
                if (selected.Teacher.Seances.Count == 0)
                {
                    p =(selected.Periode==0||selected.Periode==2)? AnyOrDefault(_dayPereiods , i => 0.5):
                        AnyOrDefault(_fIrstHafls, ss=>rnd2.NextDouble());
                }

                else
                {
                    if (selected.Periode == 0 || selected.Periode == 2)
                    {
                        var selected1 = selected;
                        var tmp = peridos.Where(x => selected1.Teacher.Seances.All(y => y.Number != x)).ToList();
                        p = rnd2.Next(0, tmp.Count() - 1);
                        p = tmp[p];
                    }
                    else
                    {
                        var selected1 = selected;
                        var tmp = _fIrstHafls.Where(x => selected1.Teacher.Seances.All(y => y.Number != x)).ToList();
                        p = rnd2.Next(0, tmp.Count() - 1);
                        p = tmp[p];
                    }
                }
                var ff = selected.Seance;
                //int? rr = selected.ClassRoomId;
                ////var index1 = index;

                //ins = rnd2.Next(0, _rooms.Count(x => x.ClassRoomTypeId == selected.ClassRoomTypeId) - 1);
                selected.Seance = p;
                //selected.ClassRoomId = _rooms[ins].Id;
                var s1 = CalculFitness(solution);
                var f1 = s1;
                //if (f1==0) break;
                f = f1 > f0 && f1 > 0;
                selected.Seance = ff;
                //selected.ClassRoomId = rr;

            } while (f);
            index = solution.IndexOf(selected);
            solution[index].Seance = p;
            //solution[index].ClassRoomId = _rooms[ins].Id;
           
          
            return solution;
        }

       
      

        private bool P(int newValue, int oldValue)
        {
            var rnd = new Random();
            var ac = Math.Exp((newValue - oldValue)/_temperature);
            return rnd.NextDouble() > ac;

        }

        static T AnyOrDefault<T>(IList<T> e, Func<T, double> weightSelector)
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
                var w = Math.Abs(sum) <= 0
                    ? 1 / (double)e.Count
                    : weights[i] / sum;
                if (rnd < w)
                    return e[i];
                rnd -= w;
            }
            throw new Exception("Should not happen");
        }

        private int CalculFitness(ObservableCollection<Lecture> solution)
        {
            int final = 0;
            foreach (var tc in solution.Where(x=>x.GroupeId!=null).GroupBy(x => x.GroupeId))
            {
                for (int i = 1; i <= 36; i++)
                {
                    var f = tc.Count(x => x.Seance == i);
                    if (f >1) final++;
                }

            }
            foreach (var tc in solution.GroupBy(x => x.SectionId))
            {
                for (int i = 1; i <= 36; i++)
                {
                    var f = tc.Count(x => x.Seance == i);
                    //var l = tc.SelectRoomLecture(x => x.Seance == i);
                    var f1 = tc.Count(w => w.GroupeId !=null && w.Seance == i);
                    //var f3 = tc.Count(x => x.Seance == i);
                    if (f1 > 0 && f > f1)
                    {
                        final ++;
                    }
                    if (f1 == 0 && f > 1)
                    {
                        final++;
                    }
                   
                }
            }
            foreach (var lecture in solution.GroupBy(x => x.TeacherId))
            {
                for (int i = 1; i <= 36; i++)
                {
                    if (lecture.Count(w => w.Seance == i) > 1) final++;
                }
            }
            foreach (var lecture in solution.GroupBy(x => x.ClassRoomId))
            {
                for (int i = 1; i <= 36; i++)
                {
                    if (lecture.Count(w => w.Seance == i) > 1) final++;
                }
            }
        
            return final;
        }

    }
}
