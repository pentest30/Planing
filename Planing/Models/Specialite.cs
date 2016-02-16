using System.ComponentModel.DataAnnotations.Schema;

namespace Planing.Models
{
    public class Specialite
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int FilliereId { get; set; }
        [ForeignKey("Departement")]
        public int DepartementId { get; set; }
        public int FaculteId { get; set; }
        //public int AnneeId { get; set; }
        public int NiveauId { get; set; }
        public Niveau Niveau { get; set; }
        public Departement Departement { get; set; }
        public Filliere Filliere { get; set; }

        //public Annee Annee { get; set; }
        public Faculte Faculte { get; set; }
    }
}
