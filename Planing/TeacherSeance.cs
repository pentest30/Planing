using System.Collections.Generic;
using System.Linq;
using Planing.Models;

namespace Planing
{
    public class TeacherSeance
    {
        public int TeacherId { get; set; }
        public int[] Seances { get; set; }
        static readonly DbModel Db = new DbModel();

        public static List<TeacherSeance> GenerateTeacherSeances(List<int> ids , int s, int ann)
        {
            var list = new List<TeacherSeance>();
            foreach (var id in ids)
            {
                var item = new TeacherSeance();
                item.TeacherId = id;
                var ss = Db.Seances.Where(w => w.TeacherId == id&& w.Semestre ==s&& w.AnneeScolaireId ==ann).ToList();
                item.Seances = GenerateSeancesT(ss);
                list.Add(item);
            }
            return list;
        }

        private static int[] GenerateSeancesT(IReadOnlyCollection<Seance> list)
        {
            var result=  new List<int>();
            for (int i = 0; i < 36; i++)
            {
                var j = i + 1;
                result.Add( j);
            }
            if (list.Count > 0)
            {
                foreach (var seance in list)
                {
                    result.Remove(seance.Number);
                }
            }
            return result.ToArray();
        }
    }
}