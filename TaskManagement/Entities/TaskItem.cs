using System;

namespace TaskManagement.Api.Entities
{
    public class TaskItem
    {
        public long Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public long StatusId { get; set; }

        public long UserId { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;

        public Lookup Status { get; set; } = null!;
    }
}
