namespace Planing.Models
{
    public class Specialite
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FaculteId { get; set; }
        public int AnneeId { get; set; }
        public int NiveauId { get; set; }
        public Niveau Niveau { get; set; }
        public Annee Annee { get; set; }
        public Faculte Faculte { get; set; }
    }
}
