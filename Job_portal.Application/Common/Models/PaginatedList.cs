using Microsoft.EntityFrameworkCore;

namespace Job_portal.Application.Common.Models;

public class PaginatedList<T>
{
    public List<T>? Items { get; }
    public int TotalCount { get; }//total records in DB
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalPages { get; }//Total Pages
    public bool HasPriviousPage { get; }
    public bool HasNextPage { get; }

    public PaginatedList(List<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        HasPriviousPage = PageNumber > 1;
        HasNextPage = PageNumber < totalCount;
    }

    public static async Task<PaginatedList<T>> CreateAsync(
        IQueryable<T> source,
        int pageNumber,
        int pageSize, CancellationToken ct)
    {
        var totalCount = await source.CountAsync(ct);

        var items = await source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

        return new PaginatedList<T>(items,totalCount,pageNumber,pageSize);
    }

}

