using Planing.Core.DbImport;
using Planing.Core.Models;

namespace Planing.Models
{
    public class Course
    {
        public int Id { get; set; }
        [DbColumn("Matière")]
        public string Name { get; set; }
         [DbColumn("Abréviation")]
        public string Code { get; set; }
        public int? CourseTypeId { get; set; }
        public int SpecialiteId { get; set; }
        public int AnneeId { get; set; }
        public int Semestre { get; set; }
        public Annee Annee { get; set; }
        public Specialite Specialite { get; set; }
        public CourseType CourseType { get; set; }
        public int Periode { get; set; }

    }
    public enum Periode
    {
        FullDay,
        FirstHalf,
        LastHalf
    };
}
