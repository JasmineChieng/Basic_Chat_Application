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
        User LoginUser(string username);

        [OperationContract]
        bool CreateGroupChat(ChatGroup chatGroup, User user);


        [OperationContract]
        void SaveChatGroups();

        [OperationContract]
        List<ChatGroup> LoadChatGroups();

        [OperationContract]
        void handleMessage(ChatGroup chatGroup, ChatMessage chatMessage);

        [OperationContract]
        void handlePrivateMessage(User messagingUser, User receivingUser, PrivateMessage newMessage);

        [OperationContract]
        bool handleLeaveGroup(ChatGroup chatGroup, User user);

        [OperationContract]
        List<ChatMessage> LoadChatHistory(ChatGroup chatGroup);
        [OperationContract]
        List<PrivateMessage> LoadPMHistory(User messagingUser);

        [OperationContract]
        List<User> LoadChatGroupMembers(ChatGroup chatgroup);

        [OperationContract]
        bool JoinGroupChat(ChatGroup chatGroup, User user);

        [OperationContract]
        User GetUser(String memberUsername);

        [OperationContract]
        void SaveUserData();

        [OperationContract]
        List<User> LoadUserData();

        [OperationContract]
        byte[] CompressData(byte[] data);
        [OperationContract]
        byte[] DecompressData(byte[] compressedData);

    }
}
