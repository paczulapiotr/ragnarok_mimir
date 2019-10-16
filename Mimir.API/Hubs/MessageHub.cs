using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mimir.API.Hubs
{
    public class MessageHub : Hub
    {
        [HubMethodName("Send")]
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReveiveMessage", message);
        }
    }
}
