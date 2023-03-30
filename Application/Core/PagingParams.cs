namespace Application.Core
{
    public class PagingParams
    {
        private const int MaxPageSize = 50; // this depends on our app itself. It is the maximum number of records we will display per page.        
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        // hence, we show 10 records by default
        // if client puts > 50 as a page size, we will show 50 as that is the max
        // else, we show how much the client requested for
        public int PageSize
        {
            get => _pageSize; // return _pageSize
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

    }
}