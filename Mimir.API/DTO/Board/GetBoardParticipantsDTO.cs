namespace Mimir.API.DTO
{
    public class GetBoardParticipantsDTO
    {
        public string Name { get; set; }
        public int[] IgnoreUserIds { get; set; }
        public int? BoardId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
