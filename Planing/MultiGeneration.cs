using System.Collections.Generic;
using Planing.Models;

namespace Planing
{
    public class MultiGeneration
    {
        public double CountLateTime { get; set; }
        public int CountConflict { get; set; }
        public List<Lecture> Lectures { get; set; }
    }
}
