using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Mimir.Kanban;

namespace Mimir.API.Hubs
{
    public class BoardSynchronizationHub : Hub
    {
        const string SUBSCRIBE_METHOD = "Subscribe";
        private readonly IKanbanAccessService _accessService;

        static BoardSynchronizationHub()
        {
            _activeConnections = new Dictionary<string, BoardSynchronizationPoint>();
        }

        public BoardSynchronizationHub(IKanbanAccessService accessService)
        {
            _accessService = accessService;
        }

        public static ReadOnlyDictionary<string, BoardSynchronizationPoint> ActiveConnections
            => new ReadOnlyDictionary<string, BoardSynchronizationPoint>(_activeConnections);

        private static Dictionary<string, BoardSynchronizationPoint> _activeConnections { get; set; }

        [HubMethodName(SUBSCRIBE_METHOD)]
        public void Subscribe(string message)
        {
            if (int.TryParse(message, out var boardId))
            {
                StoreConnection(boardId);
            }
        }

        public override Task OnConnectedAsync()
        {
            StoreConnection();
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            RemoveConnection();
            return base.OnDisconnectedAsync(exception);
        }

        private void StoreConnection(int? boardId = null)
        {
            var authId = Context.UserIdentifier;
            var connectionId = Context.ConnectionId;

            if (boardId.HasValue && !_accessService.HasAccess(authId, boardId.Value))
                return;

            if (ActiveConnections.TryGetValue(connectionId, out var syncPoint))
            {
                syncPoint.BoardId = boardId;
            }
            else
            {
                _activeConnections.TryAdd(Context.ConnectionId,
                    new BoardSynchronizationPoint
                    {
                        UserIdentifier = authId,
                        BoardId = boardId
                    });
            }
        }

        private void RemoveConnection()
        {
            var connectionId = Context.ConnectionId;
            _activeConnections.Remove(connectionId);
        }
    }
}
