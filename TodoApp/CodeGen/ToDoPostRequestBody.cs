using System.ComponentModel.DataAnnotations;

namespace CodeGen
{
    public record ToDoPostRequestBody([Required] string Title, [Required] string Description, [Required] StatusEnum Status);
}
