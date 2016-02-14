namespace Planing.Models
{
    public class Groupe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SectionId { get; set; }
        public int Semestre { get; set; }
        public Section Section { get; set; }
    }
}
