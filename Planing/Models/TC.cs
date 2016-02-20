namespace Planing.Models
{
    public class Tc
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int CourseId { get; set; }
        public int ScheduleWieght { get; set; }
        public int AnneeScolaireId { get; set; }
        public int Semestre { get; set; }
        public int ClassRoomTypeId { get; set; }
        public AnneeScolaire AnneeScolaire { get; set; }  
        public Teacher Teacher { get; set; }
        public Course Course { get; set; }
        public int? SectionId { get; set; }
        public int? GroupeId { get; set; }
        public Section Section { get; set; }
        public Groupe Groupe { get; set; }
    }
}
