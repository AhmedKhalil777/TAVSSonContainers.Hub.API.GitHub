using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hub.API.Contracts.V1
{
    public class ApiRoutes
    {
        public const string Root = "api/";

        public const string Version = "v1/";

        public const string Base = Root + Version ;

        public static class Chat
        {
            public const string ChatBase = Base + "chat/";

            public const string GetChannel = ChatBase + "GetChannel/{CID}";
            public const string GetUserChannels = ChatBase + "GetUserChannels/{UID}";
            public const string SendMessage = ChatBase + "SendMessage";
            public const string CreateChannel = ChatBase+ "CreateChannel";
            public const string DeleteChannel = ChatBase + "DeleteChannel/{CID}";
            public const string ModifyChannel = ChatBase + "ModifyChannel/{CID}";
            public const string CreateUser = Base + "CreateUser/";
            public const string AddUserToChannel = ChatBase + "AddUserToChannel/{CID}/{UID}";
            public const string GetUsers = Base + "GetUsers/";
            public const string GetUser = Base + "GetUser/{UID}";
            public const string SearchUser = Base + "SearchUser/{filter}";
            public const string InsertImgtoChannel = ChatBase + "InsertImgtoChannel/{CID}";
            public const string InsertImgtoUser = ChatBase + "InsertImgtoUser/{UID}";


        }
        public  static class Notification
        {
            public const string NotiBase = Base + "Noti/";
        }

        public static class Chart
        {
            public const string ChartBase = Base + "Chart/";
            public const string GetChart = ChartBase;

        }


    }
}
