using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDLL;

namespace DatabaseServer
{
    public class ServerInterfaceImpl : ServerInterface
    {
        private List<User> users = new List<User>();
        private List<ChatGroup> chatGroups = new List<ChatGroup>();

        public bool RegisterUser(User user)
        {
            // Check if the username is already taken
            if (users.Any(u => u.Username == user.Username))
            {
                return false; // Username already exists
            }

            // Store the user in the list (you should use a database here)
            users.Add(user);
            return true; // Registration successful
        }

        public bool LoginUser(string username, string password)
        {
            // Check if the user exists and the password matches (you should use a secure password hashing mechanism)
            var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);
            return user != null;
        }

        public bool CreateGroupChat(ChatGroup chatGroup)
        {
            // Check if the chat group name is unique
            if (chatGroups.Any(c => c.Name == chatGroup.Name))
            {
                return false; // Chat group name already exists
            }

            // Store the chat group in the list (you should use a database here)
            chatGroups.Add(chatGroup);
            return true; // Chat group creation successful
        }

        public bool JoinGroupChat(string joinCode, string username)
        {
            /*
            // Find the chat group with the given join code
            var chatGroup = chatGroups.FirstOrDefault(c => c.JoinCode == joinCode);

            // If the chat group exists, add the user to it
            if (chatGroup != null)
            {
                chatGroup.Members.Add(username);
                return true; // User joined the chat group
            }
*/
            return false; // Chat group not found
            
        }

        public List<ChatGroup> GetAllGroupChats()
        {
            return chatGroups;
        }

        public List<string> GetUsersInChat(string chatName)
        {
            /*
            // Find the chat group with the given name
            var chatGroup = chatGroups.FirstOrDefault(c => c.Name == chatName);

            if (chatGroup != null)
            {
                return chatGroup.Members;
            }
            */
            return new List<string>(); // Return an empty list if the chat group is not found
        }

        // Other methods for managing group chats and user data
    }
}
