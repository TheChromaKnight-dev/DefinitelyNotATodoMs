namespace BusinessLogic.DataManager
{
    public class QueryByPageGenericResult<TDao>
    {
        public string? ContinuationToken { get; set; }
        public List<TDao> Results { get; set; }
    }
}
