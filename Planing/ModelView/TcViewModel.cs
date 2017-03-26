using AutoMapper;

namespace Planing.ModelView
{
    public class TcViewModel
    {
        public int Id { get; set; }
        public int SpecialiteId { get; set; }
        public int AnneeId { get; set; }
        public int TeacherId { get; set; }
        public int CourseId { get; set; }
        public string Course { get; set; }
        public int ScheduleWieght { get; set; }
       
        public int AnneeScolaireId { get; set; }
        public int Semestre { get; set; }
        public int ClassRoomTypeId { get; set; }
        public int Periode { get; set; }
        public int SectionId { get; set; }
         [IgnoreMap]
        public string AnneeScolaire { get; set; }
         [IgnoreMap]
        public string Teacher { get; set; }
         [IgnoreMap]
        public string Section { get; set; }
         [IgnoreMap]
        public string Groupe { get; set; }
         [IgnoreMap]
        public string ClassRoomtype { get; set; }
         [IgnoreMap]
        public string Specialite { get; set; }
        public int? GroupeId { get; set; }
    }
}
