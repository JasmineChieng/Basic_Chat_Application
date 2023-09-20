using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserDLL
{
    public class ChatGroup
    {
        public string Name { get; set; }
        public bool IsPrivate { get; set; }

        public string JoinCode { get; set; }
        public List<string> Members { get; set; }
    }
}
