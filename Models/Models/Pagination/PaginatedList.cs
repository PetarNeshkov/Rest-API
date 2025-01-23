namespace Models.Pagination;

public class PaginatedList<T>
{
    public int Page { get; init; }
    
    public int PageSize { get; init; }
    
    public int TotalCount { get; init; }
    
    public int TotalPages { get; init; }
    
    public List<T>? Items { get; init; }
}