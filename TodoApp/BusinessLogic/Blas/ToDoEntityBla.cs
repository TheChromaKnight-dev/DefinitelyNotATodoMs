using BusinessLogic.Enums;

namespace BusinessLogic.Blas
{
    public record ToDoEntityBla(string Id, string Title, string Description, StatusBlaEnum Status)
    {
        public StatusBlaEnum Status { get; set; } = Status;
        public string Description { get; set; } = Description;
    }
}
