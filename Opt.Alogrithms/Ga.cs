using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;
using Planing.Core.Models;

namespace Opt.Alogrithms
{
    public sealed class Ga : ChromosomeBase
    {
        public static Sa Sa = new Sa();
        static  List<Lecture> _lectures = Sa.Run();
        
        public Ga() : base(Count())
        {


            GenerateGenes();
            CreateGenes();
        }

        void GenerateGenes()
        {
           
            _lectures =  new List<Lecture>();
            _lectures.AddRange(Sa.Run());
        }
        private static int Count()
        {
           
            return _lectures.Count;
        }

        public override Gene GenerateGene(int geneIndex)
        {
            return new Gene(_lectures[geneIndex]);

        }

        public Gene GenerateGene(List<Lecture> lectures, int index)
        {
            return new Gene(lectures[index]);
        }

        public override IChromosome CreateNew()
        {
            return new Ga();
        }
    }
}
