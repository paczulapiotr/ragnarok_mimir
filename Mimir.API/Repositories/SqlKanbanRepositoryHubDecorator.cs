using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Mimir.API.Hubs;
using Mimir.API.Services;
using Mimir.Kanban;

namespace Mimir.API.Repositories
{
    public class SqlKanbanRepositoryHubDecorator : IKanbanRepository
    {
        private readonly SqlKanbanRepository _kanbanRepository;
        private readonly IHubContext<BoardSynchronizationHub> _hubContext;
        private readonly IContextUserResolver _contextUserResolver;
        private int _currentUserId => _contextUserResolver.GetCurrentUser().ID;

        public SqlKanbanRepositoryHubDecorator(
            SqlKanbanRepository kanbanRepository, 
            IHubContext<BoardSynchronizationHub> hubContext,
            IContextUserResolver contextUserResolver)
        {
            _kanbanRepository = kanbanRepository;
            _hubContext = hubContext;
            _contextUserResolver = contextUserResolver;
        }
        

        public async Task AddColumnAsync(int boardId, string name)
        {
            await _kanbanRepository.AddColumnAsync(boardId, name);
            _hubContext.NotifyOnBoardUpdate(_currentUserId, boardId);
        }

        public async Task AddItemAsync(int boardId, string name, int columnId)
        {
            await _kanbanRepository.AddItemAsync(boardId, name, columnId);
            _hubContext.NotifyOnBoardUpdate(_currentUserId, boardId);
        }

        public async Task EditColumnAsync(int boardId, int columnId, string name)
        {
            await _kanbanRepository.EditColumnAsync(boardId, columnId, name);
            _hubContext.NotifyOnBoardUpdate(_currentUserId, boardId);
        }

        public async Task EditItemAsync(int boardId, int itemId, string name, string description, int? assigneeId)
        {
            await _kanbanRepository.EditItemAsync(boardId, itemId, name, description, assigneeId);
            _hubContext.NotifyOnBoardUpdate(_currentUserId, boardId);
        }

        public async Task MoveColumnAsync(int boardId, int columnId, int index, DateTime timestamp)
        {
            await _kanbanRepository.MoveColumnAsync(boardId, columnId, index, timestamp);
            _hubContext.NotifyOnBoardUpdate(_currentUserId, boardId);
        }

        public async Task MoveItemAsync(int boardId, int itemId, int index, int? destColumnId, DateTime timestamp)
        {
            await _kanbanRepository.MoveItemAsync(boardId, itemId, index, destColumnId, timestamp);
            _hubContext.NotifyOnBoardUpdate(_currentUserId, boardId);
        }

        public async Task RemoveColumnAsync(int boardId, int columnId)
        {
            await _kanbanRepository.RemoveColumnAsync(boardId, columnId);
            _hubContext.NotifyOnBoardUpdate(_currentUserId, boardId);
        }

        public async Task RemoveItemAsync(int boardId, int itemId)
        {
            await _kanbanRepository.RemoveItemAsync(boardId, itemId);
            _hubContext.NotifyOnBoardUpdate(_currentUserId, boardId);
        }
    }
}
