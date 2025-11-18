namespace TaskManagement.Api.Entities
{
    public class User
    {
        public long Id { get; set; }

        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string HashedPassword { get; set; } = null!;

        // Navigation property - كل يوزر إله مهام
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
