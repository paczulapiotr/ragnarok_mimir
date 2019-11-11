using System.Collections.Generic;

namespace Mimir.API.DTO
{
    public class CreateBoardDTO
    {
        public string Name { get; set; }
        public IEnumerable<int> ParticipantIds{ get; set; }
    }
}
