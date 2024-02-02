using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D1WebApp.BusinessLogicLayer
{
    public class Message
    {
        public long ID {get; set;}
        public long GroupNo { get; set; }
        public string MessageType { get; set; }
        public string MessageDescription { get; set; }
        
    }
}