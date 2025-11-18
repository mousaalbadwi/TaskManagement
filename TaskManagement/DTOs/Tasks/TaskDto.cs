namespace TaskManagement.Api.DTOs.Tasks
{
    public class TaskDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long StatusId { get; set; }
        public string StatusName { get; set; } = null!;
    }
}
