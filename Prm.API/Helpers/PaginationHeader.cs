namespace Prm.API.Helpers
{
    public class PaginationHeader
    {
        public int CurrentPage { get; set; }
        public int itemsPerPage { get; set; }
        public int TotalItems { get; set; }
        public int Total { get; set; }

        public PaginationHeader(int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            this.CurrentPage = currentPage;
            this.itemsPerPage = itemsPerPage;
            this.TotalItems = totalItems;
            this.Total = totalPages;
        }
    }
}