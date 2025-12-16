namespace NotionAPI.Models
{
    public class Tasks
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

        public int UserId { get; set; }
        public virtual Users Users { get; set; }
    }
}
