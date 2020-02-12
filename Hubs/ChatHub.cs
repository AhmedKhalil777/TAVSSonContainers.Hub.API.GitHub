using Hub.API.Contracts.V1.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hub.API.Hubs
{
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public async Task BroadCast(string Sender , string Message ) 
            => await Clients.All.SendCoreAsync("BroadcastedMessage",new string[] { Sender, Message });
        

        public async void GroupCast(string Sender , string GroupId, string Message)
            => await Clients.Group(GroupId).SendCoreAsync(GroupId, new[] { new MessageContainerViewModel(){ 
                Body = Message ,CID = GroupId ,UID =Sender } });
        

        public async Task UniCast(string Sender, string RecieverId, string Message)
            => await Clients.User(RecieverId).SendCoreAsync(RecieverId, new string[] { Sender, Message });
        
    }
}
