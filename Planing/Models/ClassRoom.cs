namespace Planing.Models
{
    public class ClassRoom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public int MinSize { get; set; }
        public int MaxSize { get; set; }
        public int FaculteId { get; set; }
        public Faculte Faculte { get; set; }
    }
}
