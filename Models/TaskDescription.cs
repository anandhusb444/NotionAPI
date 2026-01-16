namespace NotionAPI.Models
{
    public class TaskDescription
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int TaskTd { get; set; }
        public TodoTasks TodaTask { get; set; }

    }
}
