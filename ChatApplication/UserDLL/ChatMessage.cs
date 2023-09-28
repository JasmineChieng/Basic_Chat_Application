using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserDLL
{
    public class ChatMessage
    {
        public String Sender { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }

        public List<FileAttachment> Attachments { get; set; } 

        public ChatMessage()
        {
            Attachments = new List<FileAttachment>();
        }


    }
}
