using Planing.Core.DbImport;
using Planing.Models;

namespace Planing.Core.Models
{
    public class ClassRoom
    {
      [DbColumn("Id")]
        public int Id { get; set; }
        [DbColumn("Name")]
        public string Name { get; set; }
        [DbColumn("Code")]
        public string Code { get; set; }
        public string Type { get; set; }
        public int MinSize { get; set; }
        public int MaxSize { get; set; }
        public int FaculteId { get; set; }
        [DbColumn("ClassRoomTypeId")]
        public int ClassRoomTypeId { get; set; }
        public Faculte Faculte { get; set; }
        public ClassRoomType ClassRoomType { get; set; }
    }
}
