using BusinessLogic.Enums;

namespace BusinessLogic.Blas
{
    public record ToDoPostRequestBodyBla(string Title, string Description,StatusBlaEnum Status);
}
