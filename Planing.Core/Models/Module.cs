using Planing.Core.DbImport;

namespace Planing.Core.Models
{
    public class Module
    {
        public string Nom { get; set; }
        [DbColumn("Matière")]
        public string Code { get; set; }
        [DbColumn("Classe")]
        public string Specialite { get; set; }
    }
}
