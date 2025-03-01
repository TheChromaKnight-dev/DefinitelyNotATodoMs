using BusinessLogic.Enums;

namespace BusinessLogic.Blas
{
    public record ToDoChangeRequestBla(string Id, StatusBlaEnum? Status, string? Description);
}
