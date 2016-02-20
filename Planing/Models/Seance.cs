using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Planing.Models
{
    public class Seance
    {
        public int Id { get; set; }
       
        //[ForeignKey("AnneeScolaire")]
        public int AnneeScolaireId { get; set; }
        public int Number { get; set; }
        public int Day { get; set; }
        public string HourStart { get; set; }
        public string HourEnd { get; set; }
        public int TeacherId { get; set; }
        [DefaultValue(1)]
        public int  Semestre { get; set; }
        public Teacher Teacher { get; set; }
        public AnneeScolaire AnneeScolaire { get; set; }

    }
}
