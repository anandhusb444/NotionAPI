namespace NotionAPI.DTOs.Task
{
    public record TaskDto(int? taskId,string Title,string Description,bool IsCompleted);

}
