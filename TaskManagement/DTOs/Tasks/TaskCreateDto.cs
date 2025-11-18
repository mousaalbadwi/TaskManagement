namespace TaskManagement.Api.DTOs.Tasks
{
    public class TaskCreateDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long StatusId { get; set; }
    }
}
