using System.Collections.Generic;
using Planing.Core.DbImport;

namespace Planing.Core.Models
{
    public class Teacher
    {
        public Teacher()
        {
            Seances = new List<Seance>();
        }
        public int Id { get; set; }
        [DbColumn("Nom")]
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public int  Numero { get; set; }
        public int FaculteId { get; set; }
        public Faculte Faculte { get; set; }
        public List<Seance> Seances { get; set; }
        public string FullName
        {
            get { return Nom + " " + Prenom; }
        }
    }
}
