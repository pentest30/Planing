using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using Planing.Core.Models;

namespace Opt.Alogrithms
{
    public class PlFitness : IFitness
    {
        public double SoftFitness(IChromosome chromosome)
        {
            var chromosom = chromosome.GetGenes().Select(x => x.Value).OfType<Lecture>();
            int final = 0;
            var enumerable = chromosom as Lecture[] ?? chromosom.ToArray();
            foreach (var lecture in enumerable)
            {
                if (CheckIfExistInFirstPeriode(lecture.Seance)) final += 10;
                if (CheckIfExistForSecondPeriode(lecture.Seance)) final -= 5;
            }
            return final;
        }
        public double Evaluate2(IChromosome chromosome)
        {
            var chromosom = chromosome.GetGenes().Select(x => x.Value).OfType<Lecture>();
            int final = 0;
            var enumerable = chromosom as Lecture[] ?? chromosom.ToArray();
            foreach (var tc in enumerable.GroupBy(x => x.SectionId))
            {
                for (int i = 1; i <= 36; i++)
                {
                    var f = tc.Count(x => x.Seance == i && x.GroupeId == 0);
                    //var l = tc.SelectRoomLecture(x => x.Seance == i);
                    var f1 = tc.Count(w => w.GroupeId != 0 && w.Seance == i);
                    //var f3 = tc.Count(x => x.Seance == i);
                    if (f1 > 0 && f > f1)
                    {
                        final++;
                    }
                    else if (f1 == 0 && f > 1)
                    {
                        final++;
                    }
                }


            }
            foreach (var lecture in enumerable.GroupBy(x => x.TeacherId))
            {
                for (int i = 1; i <= 36; i++)
                {
                    if (lecture.Count(w => w.Seance == i) > 1) final++;
                }
            }

            return -final;
        }
        public double Evaluate(IChromosome chromosome)
        {
            var chromosom = chromosome.GetGenes().Select(x => x.Value).OfType<Lecture>();
            int final = 0;
            var enumerable = chromosom as Lecture[] ?? chromosom.ToArray();
            foreach (var tc in enumerable.GroupBy(x => x.SectionId))
            {
                for (int i = 1; i <= 36; i++)
                {
                    var f = tc.Count(x => x.Seance == i && x.GroupeId == 0);
                    //var l = tc.SelectRoomLecture(x => x.Seance == i);
                    var f1 = tc.Count(w => w.GroupeId != 0 && w.Seance == i);
                    //var f3 = tc.Count(x => x.Seance == i);
                    if (f1 > 0 && f > f1)
                    {
                        final++;
                    }
                    else if (f1 == 0 && f > 1)
                    {
                        final++;
                    }
                }
              

            }
            foreach (var lecture in enumerable.GroupBy(x => x.TeacherId))
            {
                for (int i = 1; i <= 36; i++)
                {
                    if (lecture.Count(w => w.Seance == i) > 1) final++;
                }
            }
          
            return -final + SoftFitness(chromosome);
        }
        private static bool CheckIfExistForSecondPeriode(int s)
        {
           var secondeHafls = new[] { 4, 5, 6, 10, 11, 12, 16, 17, 18, 22, 23, 24, 25, 28, 29, 30, 34, 35, 36 };
            return secondeHafls.Any(x => x == (s));
        }

        private static bool CheckIfExistInFirstPeriode(int s)
        {
            var firstHafls = new[] { 1, 2, 3, 7, 8, 9, 13, 14, 15, 19, 20, 21, 25, 26, 27, 31, 32, 33 };
            return firstHafls.Any(x => x == (s));

        }

    }
}
