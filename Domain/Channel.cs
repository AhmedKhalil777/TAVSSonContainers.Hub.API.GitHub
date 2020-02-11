using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hub.API.Domain
{
    public class Channel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ChannelId { get; set; }
        public string AdminId { get; set; }
        public string ImgPath { get; set; }
        public string Caption { get; set; }
        public bool Status { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; }
        public List<Message> Messages { get; set; }
    }
}
