using DatabaseServer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UserDLL;

namespace BusinessTierServer
{
    internal class BusinessInterfaceImpl : BusinessInterface
    {
        private List<User> users = new List<User>();
        private List<ChatGroup> chatGroups = new List<ChatGroup>();

/*         public BusinessInterfaceImpl()
        {
            users = LoadUserData();
            chatGroups = LoadChatGroups();
        }*/

        private ServerInterface foob;

        public BusinessInterfaceImpl()
        {

            ChannelFactory<ServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            string URL = "net.tcp://localhost:8100/DataService";
            foobFactory = new ChannelFactory<ServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();


        }

        public bool RegisterUser(User user)
        {
         

            return foob.RegisterUser(user);

        }
        

        public User LoginUser(string username)
        {

            return foob.LoginUser(username);
        }
        public bool CreateGroupChat(ChatGroup chatGroup, User user)
        {
    
            return foob.CreateGroupChat(chatGroup, user);
        }

        public bool JoinGroupChat(ChatGroup chatGroup, User user)
        {
            return foob.JoinGroupChat(chatGroup, user);
    
        }

        public User GetUser(String memberUsername)
        {
            return foob.GetUser(memberUsername);
        }



        public void SaveChatGroups()
        {
             foob.SaveChatGroups();
  
        }


        public List<ChatGroup> LoadChatGroups()
        {
            return foob.LoadChatGroups();
  
        }


        public void SaveUserData()
        {
               foob.SaveUserData();

        }

        // Load user data from the JSON file
        public List<User> LoadUserData()
        {
            return foob.LoadUserData();
   
        }

        public void handleMessage(ChatGroup chatGroup,ChatMessage newMessage)
        {
    
            foob.handleMessage(chatGroup, newMessage);
        }

        public void handlePrivateMessage(User messagingUser, User receivingUser, PrivateMessage newMessage)
        {
            foob.handlePrivateMessage(messagingUser, receivingUser, newMessage);
        }

        public List<ChatMessage> LoadChatHistory(ChatGroup chatGroup)
        {
            return foob.LoadChatHistory(chatGroup);
   
        }

        public bool handleLeaveGroup(ChatGroup chatgroup, User user)
        {
           return foob.handleLeaveGroup(chatgroup, user); 

        }

        public List<User> LoadChatGroupMembers(ChatGroup chatgroup)
        {
            return foob.LoadChatGroupMembers(chatgroup);
    
        }

        byte[] BusinessInterface.CompressData(byte[] data)
        {
           return foob.CompressData(data);
        }

        byte[] BusinessInterface.DecompressData(byte[] compressedData)
        {
           return foob.DecompressData(compressedData);  
        }
    }
}
