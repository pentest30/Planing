using System.ComponentModel.DataAnnotations.Schema;
using Planing.Core.DbImport;
using Planing.Models;

namespace Planing.Core.Models
{
    public class Tc
    {
        //[ReadOnly(true)]
        //[IgnoreMap]
        public int Id { get; set; }
        [DbColumn("TeacherId")]
        public int TeacherId { get; set; }
        [DbColumn("CourseId")]
        public int CourseId { get; set; }
        [DbColumn("ScheduleWieght")]
        public int ScheduleWieght { get; set; }
        [DbColumn("AnneeScolaireId")]
        public int AnneeScolaireId { get; set; }
        [DbColumn("Semestre")]
        public int Semestre { get; set; }
        [DbColumn("ClassRoomTypeId")]
        public int ClassRoomTypeId { get; set; }
        public int Periode { get; set; }
     //    [IgnoreMap]
        public AnneeScolaire AnneeScolaire { get; set; }
       //  [IgnoreMap]
        public Teacher Teacher { get; set; }
       //  [IgnoreMap]
        public Course Course { get; set; }
     [DbColumn("SectionId")]
        public int SectionId { get; set; }
     [NotMapped]
        public int SeanceSum { get; set; }
       [DbColumn("GroupeId")]
        public int? GroupeId { get; set; }
     //    [IgnoreMap]
        public Section Section { get; set; }
       //  [IgnoreMap]
        public Groupe Groupe { get; set; }
      //   [IgnoreMap]
        public ClassRoomType ClassRoomType { get; set; }
    }
}
