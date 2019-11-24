using System.Collections.Generic;

namespace Mimir.API.DTO
{
    public class EditBoardParticipantsRequestDTO
    {
        public int Id { get; set; }
        public IEnumerable<int> ParticipantIds { get; set; }
    }
}
