using Planing.Models;

namespace Planing.Core.Models
{
    public class Departement
    {
        public int Id { get; set; }
        public string Libelle { get; set; }
        public int FaculteId { get; set; }
        public Faculte Faculte { get; set; }
    }
}
