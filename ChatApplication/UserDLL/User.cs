using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserDLL
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public List<ChatGroup> JoinedGroups { get; set; } = new List<ChatGroup>();
        public List<PrivateMessage> PrivateMessages { get; set; } = new List<PrivateMessage>();
    }

}
