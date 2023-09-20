using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using UserDLL;



namespace DatabaseServer
{
    [ServiceContract]
    internal interface ServerInterface
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
