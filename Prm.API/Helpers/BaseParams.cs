namespace Prm.API.Helpers
{
    public abstract class BaseParams
    {
          private const int MaxSizePage = 50;
        public int PageNumber { get; set; } = 1;
        private int size = 10;
        public int SizePage
        {
            get { return size;}
            set { size = (value > MaxSizePage) ? MaxSizePage : value;}
        }

        public int UserId { get; set; }
    }
}