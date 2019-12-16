using System.Linq;
using Microsoft.AspNetCore.SignalR;

namespace Mimir.API.Hubs
{
    public static class BoardSynchronizationHubExtensions
    {
        const string UPDATE_BOARD_METHOD = "UpdateBoard";

        public static void NotifyOnBoardUpdate(this IHubContext<BoardSynchronizationHub> @this, int boardId)
        {
            var connections = BoardSynchronizationHub.ActiveConnections
                .Where(x => x.Value.BoardId == boardId)
                .Select(x => x.Key)
                .ToList();

            @this.Clients.Clients(connections).SendAsync(UPDATE_BOARD_METHOD, boardId);
        }
    }
}
