using System.Collections.Immutable;

namespace CodeGen
{
    public record ToDoByPageResponse(string? ContinuationToken, ImmutableArray<ToDoResponse> Results);
}
