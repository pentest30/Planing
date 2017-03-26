using System;
using System.Collections.Generic;
using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Randomizations;
using Planing.Core.Models;

namespace Opt.Alogrithms
{
    public class PlMutation:IMutation
    {
        readonly Sa _sa = new Sa();
        readonly List<Lecture> _tabu = new List<Lecture>(); 
        public bool IsOrdered { get; private set; }
        public void Mutate(IChromosome chromosome, float probability)
        {
         
            while (RandomizationProvider.Current.GetDouble() < probability)
            {
                Mutate(chromosome);
                // Mutate(chromosome);
                
            }
          
            // return Solution;
        }

        private void Mutate(IChromosome chromosome)
        {
            bool exist = false;
            Lecture firstOrDefault;
            var solution = chromosome.GetGenes().Select(x => x.Value).OfType<Lecture>().ToList();
            var fit =  new PlFitness();

            double f = 0; 
            var rnd2 = new Random();
          
            do
            {
                var rnd = new Random();
                int index = rnd2.Next(0, solution.Count() - 1);
                int p = rnd.Next(1, 36);
                solution[index].Seance = p;
                f = fit.Evaluate2(chromosome);

            } while (f > 0);
           
           
            
        }
    }
}
