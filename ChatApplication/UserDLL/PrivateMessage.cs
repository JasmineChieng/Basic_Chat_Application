using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserDLL
{
    public class PrivateMessage
    {
        public User Sender { get; set; }
        public User Receiver { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }


}
