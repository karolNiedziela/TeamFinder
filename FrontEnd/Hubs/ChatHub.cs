using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message, string time)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message, time);
        }
    }
}
