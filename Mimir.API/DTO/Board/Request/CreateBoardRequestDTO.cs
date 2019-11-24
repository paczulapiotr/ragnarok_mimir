using System.Collections.Generic;

namespace Mimir.API.DTO
{
    public class CreateBoardRequestDTO
    {
        public string Name { get; set; }
        public IEnumerable<int> ParticipantIds{ get; set; }
    }
}
