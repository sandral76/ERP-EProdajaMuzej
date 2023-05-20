namespace Core.Specification
{
    public class  UlazniceSpecParams
    {
        private const int MaxPageSize = 50;
        public int PageIndex { get; set; } = 1;
        private int pageSize = 6;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public string? Grad {get;set;}
        public string Sort{get;set;}
        private string search;
        public string Search {

            get=>search;
            set=>search=value.ToLower();
        }
    }
}