

using X.PagedList;

namespace EVMSWeb.ViewModels
{
    public class PagedListModel<T>
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public string prevLink { get; set; }
        public string nextLink { get; set; }
        public DateTime? Accesstime { get; set; }
        public IEnumerable<T> Results { get; set; }
    }

    public class PagingModel<T>
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public string prevLink { get; set; }
        public string nextLink { get; set; }
        public IPagedList<T> Results { get; set; }
    }
}
