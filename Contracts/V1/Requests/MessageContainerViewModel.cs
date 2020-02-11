using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hub.API.Contracts.V1.Requests
{
    public class MessageContainerViewModel
    {
        public string Body { get; set; }
        public string CID { get; set; }
        public string UID { get; set; }

    }
}
