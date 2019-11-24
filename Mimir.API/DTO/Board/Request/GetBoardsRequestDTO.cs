namespace Mimir.API.DTO
{
    public class GetBoardsRequestDTO
    {
        public bool Owned { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }
    }
}
