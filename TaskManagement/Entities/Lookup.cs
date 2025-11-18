namespace TaskManagement.Api.Entities
{
    public class Lookup
    {
        public long Id { get; set; }

        public int MajorCode { get; set; }

        public int MinorCode { get; set; }

        public string Name { get; set; } = null!;

        // Navigation - لو حبينا نربط لاحقاً
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
