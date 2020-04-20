using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WheaderRequest
{
    public class SendNotif
    {

        public interface IEventHub
        {
            // here place some method(s) for message from server to client
            Task SendNoticeEventToClient(string message);
        }

        public class EventHub : Hub<IEventHub>
        {
            // here place some method(s) for message from client to server
        }
    }
}
