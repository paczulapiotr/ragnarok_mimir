namespace Mimir.API.DTO
{
    public class SearchNewBoardParticipantsRequestDTO
    {
        public string Name { get; set; }
        public int[] IgnoreUserIds { get; set; }
        public int? BoardId { get; set; }
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 5;
    }
}
