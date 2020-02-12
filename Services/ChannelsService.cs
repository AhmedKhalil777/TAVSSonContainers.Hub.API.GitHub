using Hub.API.Contracts.V1.Requests;
using Hub.API.Contracts.V1.Responses;
using Hub.API.Domain;
using Hub.API.Hubs;
using Hub.API.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Hub.API.Hubs;
namespace Hub.API.Services
{
    public class ChannelsService : IChannelsService
    {
        private readonly IMongoCollection<Domain.Channel> _channel;
        private readonly IMongoCollection<Domain.User> _user;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IHubContext<ChatHub> _hub;
        
       
        public ChannelsService(IChannelsDatabaseSettings channelsettings, IUsersDatabaseSettings usresettings, IHubContext<ChatHub> hub , IHostEnvironment hostEnvironment)
        {
            var client = new MongoClient(channelsettings.ConnectionString);
            var database = client.GetDatabase(channelsettings.DatabaseName);
            _hub = hub;
            _channel = database.GetCollection<Domain.Channel>(channelsettings.ChannelsCollectionName);
            _user = database.GetCollection<User>(usresettings.UsersCollectionName);
            _hostEnvironment = hostEnvironment;
        }
        #region Channel and Mesaages

        public async Task<User> GetUser(string UID)
        {
            var result = await _user.FindAsync(x => x.UserId == UID);
            var user = result.FirstOrDefault();
             user.ImgPath =user.ImgPath.Replace(@"\", @"/");
            return user;
        }


        public async Task<bool> CreateChannel(Domain.Channel channel)
        {
            var Channel = new Domain.Channel()
            {
                AdminId = channel.AdminId,
                Caption = channel.Caption,
                ImgPath = "",
                Messages = new List<Message>(),
                Name = channel.Name,
                Status = false,
                Users = new List<User>() { await GetUser(channel.AdminId) },
                

            };
            try
            {
                await _channel.InsertOneAsync(Channel);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
            
        }

        public async Task<bool> DeleteChannel(string CID)
        {
            var ack = await _channel.DeleteOneAsync(x => x.ChannelId == CID);
            if (ack.IsAcknowledged)
            {
                return true;
            }
            return false;
        }

        public async Task<Domain.Channel> GetChannel(string CID) 
        {
           var result = await _channel.FindAsync(x => x.ChannelId == CID);
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<MinChannelViewModel>> GetUserChannels(string UID)
        {
            var result = await _channel.FindAsync(x =>true);
            return result.ToList().Where(x => x.Users.Exists(y => y.UserId == UID))
                .Select(x =>
                new MinChannelViewModel { Caption = x.Caption ,Name =x.Name , ChannelId=x.ChannelId,ImgPath =x.ImgPath,Status= x.Status});
        }
        #endregion

        public async Task<bool> CreateUser( User user)
        {
            try
            {
                await _user.InsertOneAsync(user);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
             
        }

 

        public async Task<IEnumerable<User>> SearchUser(string filter)
        {
            var users = await _user.FindAsync(x => x.Name.Contains(filter));

            return users.ToList();
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _user.FindAsync(x =>true);

            return users.ToList();
        }

        public async Task<bool> InsertImgtoChannel(string CID , IFormFile file)
        {
            var Channel = await GetChannel(CID);
            string m = Channel.ChannelId + Channel.Name;
            if (file.Length > 0)
            {
                try
                {

                    if (!Directory.Exists(_hostEnvironment.ContentRootPath + "\\wwwroot\\" + "Images" + "\\" + m + "\\"))
                    {
                        Directory.CreateDirectory(_hostEnvironment.ContentRootPath + "\\wwwroot\\" + "Images" + "\\" + m + "\\");
                    }
                    string guid = Guid.NewGuid().ToString();
                    using (FileStream fileStream = File.Create(_hostEnvironment.ContentRootPath + "\\wwwroot\\" + "Images" + "\\" + m + "\\" + guid + file.FileName.Replace("\\", "s").Replace(":", "s")))
                    {
                        file.CopyTo(fileStream);
                        fileStream.Flush();
                        Channel.ImgPath = "https://localhost:5001" +"\\" + "Images" + "\\" + m + "\\" + guid + file.FileName.Replace("\\", "s").Replace(":", "s");
                        var result = await _channel.ReplaceOneAsync(x => x.ChannelId == CID, Channel);
                        return result.IsAcknowledged;
                    }
                }
                catch
                {

                    return false;
                }

            }

            return false;
        }

        public async Task<bool> InsertImgtoUser(string UID , IFormFile file)
        {
             var User = await GetUser(UID);
            string m = User.UserId + User.Name;
            if (file.Length > 0)
            {
                try
                {

                    if (!Directory.Exists(_hostEnvironment.ContentRootPath + "\\wwwroot\\" + "Images" + "\\" + m + "\\"))
                    {
                        Directory.CreateDirectory(_hostEnvironment.ContentRootPath + "\\wwwroot\\" + "Images" + "\\" + m + "\\");
                    }
                    string guid = Guid.NewGuid().ToString();
                    using (FileStream fileStream = File.Create(_hostEnvironment.ContentRootPath + "\\wwwroot\\" + "Images" + "\\" + m + "\\" + guid + file.FileName.Replace("\\", "s").Replace(":", "s")))
                    {
                        file.CopyTo(fileStream);
                        fileStream.Flush();
                        User.ImgPath = "https://localhost:5001" + "\\" + "Images" + "\\" + m + "\\" + guid + file.FileName.Replace("\\", "s").Replace(":", "s");
                        var result = await _user.ReplaceOneAsync(x => x.UserId == UID, User);
                        return result.IsAcknowledged;
                    }
                }
                catch
                {

                    return false;
                }

            }

            return false;
        }

        public async Task<bool> ModifyChannel(string CID, ModifyChannelViewModel model )
        {
            var channel = await GetChannel(CID);
            channel.Caption = model.Caption;
            channel.Name = channel.Name;
            var ack = await _channel.ReplaceOneAsync( x => x.ChannelId == channel.ChannelId , channel);
            return ack.IsAcknowledged;
        }

        public async Task<bool> SendMessage(string CID, string UID , Message message)
        {
            var user = await GetUser(UID);
            message.imgPath = user.ImgPath;
            var channel = await GetChannel(CID);
            channel.Messages.Add(message);
            var ack = await _channel.ReplaceOneAsync( x=>x.ChannelId ==CID , channel );
            return ack.IsAcknowledged;
        }

        public async Task<bool> AddUserToChannel(string CID, string UID)
        {
            var user = await GetUser(UID);
            var channel = await GetChannel(CID);
            channel.Users.Add(user);
            var Ack = await _channel.ReplaceOneAsync(x => x.ChannelId == CID, channel);
            return Ack.IsAcknowledged;

        }
    }
}
