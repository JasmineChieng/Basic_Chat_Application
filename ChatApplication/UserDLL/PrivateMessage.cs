using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserDLL
{
    public class PrivateMessage
    {
        public String Sender { get; set; }
        public String Receiver { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }


}
