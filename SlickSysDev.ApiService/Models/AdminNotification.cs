namespace SlickSysDev.Data.Service.Models
{
    public class AdminNotification 
    {
        public int AppointmentId { get; set; }

        public Guid PracticeId { get; set; }

        public Guid ClientId { get; set; }

        public Guid? PrincipalId { get; set; }

        public Guid? ResourceId { get; set; }

        public Guid StatusId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Message { get; set; }

        public Guid? GroupId { get; set; }

        public Guid? RecurrenceId { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPhone { get; set; }
        public string ServiceType { get; set; }
        public string ScheduledAt { get; set; }
        public string Notes { get; set; }
        public string Source { get; set; }
        public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
        public bool IsDeleted { get; set; }
        public bool IsRead { get; set; }
        public bool Cancelled { get; set; }


        public DateTime CreatedAt { get; set; }
    }
}
