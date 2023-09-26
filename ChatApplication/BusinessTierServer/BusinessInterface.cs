using Newtonsoft.Json;
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
        // bool LoginUser(string username, string password);
        User LoginUser(string username);

        [OperationContract]
        // bool CreateGroupChat(ChatGroup chatGroup);
        bool CreateGroupChat(ChatGroup chatGroup,User user);



        [OperationContract]
        void SaveChatGroups();

        [OperationContract]
        List<ChatGroup> LoadChatGroups();

        [OperationContract]
        void handleMessage(ChatGroup chatGroup,ChatMessage chatMessage);

        [OperationContract]
        void handleLeaveGroup(ChatGroup chatGroup, User user);

        [OperationContract]
        List<ChatMessage> LoadChatHistory(ChatGroup chatGroup);

        [OperationContract]
        List<User> LoadChatGroupMembers(ChatGroup chatgroup);

        [OperationContract]
        // bool JoinGroupChat(string joinCode, string username);
        bool JoinGroupChat(ChatGroup chatGroup, User user);

     //   [OperationContract]
     //   List<ChatGroup> GetAllGroupChats();

     //   [OperationContract]
       // List<string> GetUsersInChat(string chatName);

        [OperationContract]
        void SaveUserData();

        [OperationContract]
        List<User> LoadUserData();

    }
}
