using System;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using Opt.Alogrithms;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            //var s = new SumilatedAnnealing();
            //s.Run(1000000000000);
            //var selection = new EliteSelection();
            //var crossover = new TwoPointCrossover();
            //var mutation = new PlMutation();
            //var fitness = new PlFitness();

            //var chromosome = new Ga();
            //var population = new Population(5, 10, chromosome);
            //population.GenerationStrategy = new PerformanceGenerationStrategy();

            //var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation);
            //ga.MutationProbability = 0.1f;
            //ga.Termination = new GenerationNumberTermination(10000);

            //Console.WriteLine("GA running...");
            //ga.GenerationRan += delegate
            //{

            //    var bestChromosome = ga.Population.BestChromosome;
            //    Console.WriteLine(@"Generations: {0}", ga.Population.GenerationsNumber);
            //    Console.WriteLine(@"Fitness: {0,10}", bestChromosome.Fitness);
            //    Console.WriteLine(@"Time: {0}", ga.TimeEvolving);
            //   // if (ga.BestChromosome.Fitness == 0) ga.Stop();
            //};
            //ga.Start();
        }
    }
}
