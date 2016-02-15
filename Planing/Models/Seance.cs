namespace Planing.Models
{
    public class Seance
    {
        public int Id { get; set; }
        public int AnneScolaireId { get; set; }
        public int Number { get; set; }
        public string Day { get; set; }
        public string HourStart { get; set; }
        public string HourEnd { get; set; }
        public int TeacherId { get; set; }
        public int  Semestre { get; set; }
        public Teacher Teacher { get; set; }

    }
}
