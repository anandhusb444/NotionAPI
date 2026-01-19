using NotionAPI.Models;

namespace NotionAPI.DTOs.Task
{
    public record TaskGetDto(int? taskId, string Title, ICollection<DescriptionDto> description, bool IsCompleted);

}
