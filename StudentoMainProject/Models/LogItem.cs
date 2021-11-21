using System;

namespace StudentoMainProject.Models
{
    public class LogItem
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string IPAddress { get; set; }
        public string UserAuthId { get; set; }
        public int UserId { get; set; }
        public string UserRole { get; set; }
        public string EventType { get; set; }
        public int EventParam { get; set; }

    }
}
