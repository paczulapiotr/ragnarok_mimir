using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Mimir.Database;
using Mimir.Kanban;

namespace Mimir.API.Hubs
{
    [Authorize]
    public class BoardSynchronizationHub : Hub
    {
        const string SUBSCRIBE_METHOD = "Subscribe";
        private readonly IKanbanAccessService _accessService;
        private readonly IUserResolver _userResolver;

        static BoardSynchronizationHub()
        {
            _activeConnections = new Dictionary<string, BoardSynchronizationPoint>();
        }

        public BoardSynchronizationHub(
            IKanbanAccessService accessService,
            IUserResolver userResolver)
        {
            _accessService = accessService;
            _userResolver = userResolver;
        }

        public static ReadOnlyDictionary<string, BoardSynchronizationPoint> ActiveConnections
            => new ReadOnlyDictionary<string, BoardSynchronizationPoint>(_activeConnections);

        private static Dictionary<string, BoardSynchronizationPoint> _activeConnections { get; set; }

        [HubMethodName(SUBSCRIBE_METHOD)]
        public void Subscribe(int boardId)
        {
            StoreConnection(boardId);
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
            var user = _userResolver.GetUser(Context.User);
            if (user == null)
                return;

            var connectionId = Context.ConnectionId;

            if (boardId.HasValue && !_accessService.HasAccess(user.ID, boardId.Value))
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
                        UserId = user.ID,
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
