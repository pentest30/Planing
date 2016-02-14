namespace Planing.Models
{
    public class Section
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AnneeId { get; set; }
        public int SpecialiteId { get; set; }
        public int AnneeScolaireId { get; set; }
        public int Semestre { get; set; }
        public Annee Annee { get; set; }
        public Specialite Specialite { get; set; }
        public AnneeScolaire AnneeScolaire { get; set; }
    }
}
