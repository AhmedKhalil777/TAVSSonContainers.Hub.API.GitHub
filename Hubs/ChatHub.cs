using Hub.API.Contracts.V1.Requests;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hub.API.Hubs
{
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
    {
    
        public string DomainHost { get; private set;}
        public ChatHub(IConfiguration configuration)
        {
            configuration.GetSection("HostingDomain").Bind(DomainHost);
        }
        
        public async Task BroadCast(string Sender , string Message ) 
            => await Clients.All.SendCoreAsync("RecievedMessage", new string[] { Sender, Message });
        

        public async void GroupCast(string Sender , string GroupId,string imgPath, string Message)
            => await Clients.Group(GroupId).SendCoreAsync("RecievedMessageFromGroup", new[] { new Hub.API.Domain.Message(){ 
                Body = Message ,Date= DateTime.Now ,UserId =Sender  , imgPath =imgPath.Contains("localhost")?imgPath: DomainHost+imgPath } });    
        

        public async Task UniCast(string Sender, string RecieverId, string Message)
            => await Clients.User(RecieverId).SendCoreAsync("RecievedMessage", new string[] { Sender, Message });

        public async Task JoinGroup(string CID) => await Groups.AddToGroupAsync(Context.ConnectionId, CID);

        public async Task LeaveGroup(string CID) => await Groups.RemoveFromGroupAsync(Context.ConnectionId, CID);

        
        
    }
}
