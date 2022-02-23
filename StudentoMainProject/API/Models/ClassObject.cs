namespace StudentoMainProject.API.Models
{
    public class ClassObject
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int RoomId { set; get; }
        public short Grade { set; get; } //např. 2 pro 2. A
        public string Name { set; get; } //např. A pro 2. A
    }
}
