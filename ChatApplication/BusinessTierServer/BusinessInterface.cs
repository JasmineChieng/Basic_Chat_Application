using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UserDLL;

namespace BusinessTierServer
{
    [ServiceContract]
    public interface BusinessInterface
    {
        [OperationContract]
        bool RegisterUser(User user);

        [OperationContract]
        bool LoginUser(string username, string password);

        [OperationContract]
        bool CreateGroupChat(ChatGroup chatGroup);

        [OperationContract]
        bool JoinGroupChat(string joinCode, string username);

        [OperationContract]
        List<ChatGroup> GetAllGroupChats();

        [OperationContract]
        List<string> GetUsersInChat(string chatName);
    }
}
