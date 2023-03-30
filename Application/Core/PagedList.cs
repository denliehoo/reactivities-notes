using Microsoft.EntityFrameworkCore;

namespace Application.Core
{
    public class PagedList<T> : List<T>
    {
        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            TotalCount = count;
            AddRange(items); // adds the items we get to the class that we are going to return
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            // source is a query that is going to go to our database

            // the count does make a query to our DB but only gives the count.
            // this will allow us to establish how much items is in the list
            // hence, when we doing paging, we will do 2 queries:
            // 1. the count of item 2. pagination with the items 
            var count = await source.CountAsync();
            var items = await source
                .Skip((pageNumber - 1) * pageSize) // how much records to skip
                .Take(pageSize) // how much records to take
                .ToListAsync();
            // e.g. if we want page 2 data and page size is 10,
            // we skip (2-1)*10 = 10 records and then take the next 1- records
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}