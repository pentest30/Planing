using System.ComponentModel;

namespace Planing.Core.Models
{
    public class SeanceLbrSalle
    {
        public int Id { get; set; }

        //[ForeignKey("AnneeScolaire")]
        public int AnneeScolaireId { get; set; }
        public int Number { get; set; }
        public int Day { get; set; }

        public int SalleId { get; set; }
        [DefaultValue(1)]
        public int Semestre { get; set; }
        public ClassRoom Salle { get; set; }
        public AnneeScolaire AnneeScolaire { get; set; }
    }
}
