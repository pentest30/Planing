using Planing.Core.DbImport;

namespace Planing.Core.Models
{
    public class TcModel
    {
        [DbColumn("Classe")]
        public string Section { get; set; }
        [DbColumn("Professeur")]
        public string Prof { get; set; }
    
        public string TypeClasse { get; set; }
        [DbColumn("Groupe")]
        public string Group { get; set; }
        [DbColumn("Matière")]
        public string Matiere { get; set; }
        [DbColumn("Salles de classe")]
        public string Classe { get; set; }
        [DbColumn("Nombre par semaine")]
        public int Seances { get; set; }
    }
}
