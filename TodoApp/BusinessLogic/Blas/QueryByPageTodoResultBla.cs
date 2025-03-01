using System.Collections.Immutable;

namespace BusinessLogic.Blas
{
    public record QueryByPageTodoResultBla(string? ContinuationToken, ImmutableArray<ToDoEntityBla> Results);
}
