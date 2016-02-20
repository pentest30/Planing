namespace Planing.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int CourseTypeId { get; set; }
        public int SpecialiteId { get; set; }
        public int AnneeId { get; set; }
        public int Semestre { get; set; }
        public Annee Annee { get; set; }
        public Specialite Specialite { get; set; }
        public CourseType CourseType { get; set; }
    }
}
