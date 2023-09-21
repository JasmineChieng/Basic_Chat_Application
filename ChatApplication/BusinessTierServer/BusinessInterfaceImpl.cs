using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public BusinessInterfaceImpl()
        {
            users = LoadUserData();
            chatGroups = LoadChatGroups();
        }
        public bool RegisterUser(User user)
        {
            // Check if the username is already taken
            if (users.Any(u => u.Username == user.Username))
            {
                return false; // Username already exists
            }
            // Add the new user to the list
            users.Add(user);

            // Save the modified user list to the file
            SaveUserData();


            return true; // Registration successful
        }
        
      /* public bool LoginUser(string username, string password)
        {
            // Check if the user exists and the password matches (you should use a secure password hashing mechanism)
            var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);
            
            return user != null;
        }*/

        public User LoginUser(string username)
        {
            User user = users.FirstOrDefault(u => u.Username == username);
            if (user!=null)
            { return user; }
            return null;
        }
        public bool CreateGroupChat(ChatGroup chatGroup, User user)
        {
            // Check if the chat group name is unique
            if (chatGroups.Any(c => c.Name == chatGroup.Name))
            {
                return false; // Chat group name already exists
            }  
               chatGroup.Members.Add(user);
            // Update the JoinedGroups list for all users
            foreach (User u in users)
            {
                if( u.Username.Equals( user.Username))
                {
                    u.JoinedGroups.Add(chatGroup); 
                }

            }
         
            // Store the chat group in the list
            chatGroups.Add(chatGroup);
            // Save the modified chat groups list to the file
            SaveChatGroups();
            // Save the modified user data (including JoinedGroups) to the file
            SaveUserData();

            return true; // Chat group creation successful
        }

        public bool JoinGroupChat(ChatGroup chatGroup, User user)
        {
            // Find the chat group with the given name
            var targetChatGroup = chatGroups.FirstOrDefault(c => c.Name == chatGroup.Name);

            if (targetChatGroup != null)
            {

                // Check if the user is already a member of the group
                if (!targetChatGroup.Members.Contains(user))
                {
                    targetChatGroup.Members.Add(user);
                    // Update the JoinedGroups list for all users
                
                }

                foreach (User u in users)
                {
                    if (u.Username.Equals(user.Username))
                    {
                        u.JoinedGroups.Add(targetChatGroup);
                    }

                }


                // Save the modified chat groups list to the file
                SaveChatGroups();

                // Save the modified user list to the file
                SaveUserData();

                return true;
            }



            return false; // Chat group not found
        }

        // Method to save chat groups to a JSON file
        public void SaveChatGroups()
        {
            try
            {
                string filePath = "chatGroups.json";

                // Serialize the list of chat groups to JSON format
                string jsonData = JsonConvert.SerializeObject(chatGroups, Newtonsoft.Json.Formatting.Indented);

                // Write the JSON data to the file
                File.WriteAllText(filePath, jsonData);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during file saving
                // You can log the error or take appropriate action
            }
        }

        // Method to load chat groups from a JSON file
        public List<ChatGroup> LoadChatGroups()
        {
            try
            {
                string filePath = "chatGroups.json";

                // Check if the JSON file exists
                if (File.Exists(filePath))
                {
                    // Read the JSON data from the file
                    string jsonData = File.ReadAllText(filePath);

                    // Deserialize the JSON data back to a list of chat groups
                    return JsonConvert.DeserializeObject<List<ChatGroup>>(jsonData);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during file loading
                // You can log the error or take appropriate action
            }

            return new List<ChatGroup>(); // Return an empty list if the file doesn't exist or there's an error
        }

        // Save user data to the JSON file
        public void SaveUserData()
        {
            try
            {
                string filePath = "userList.json";

                // Serialize the list of users to JSON format
                string jsonData = JsonConvert.SerializeObject(users, Newtonsoft.Json.Formatting.Indented);

                // Write the JSON data to the file
                File.WriteAllText(filePath, jsonData);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during file saving
                // You can log the error or take appropriate action
            }
        }

        // Load user data from the JSON file
        public List<User> LoadUserData()
        {
            try
            {
                string filePath = "userList.json";

                // Check if the JSON file exists
                if (File.Exists(filePath))
                {
                    // Read the JSON data from the file
                    string jsonData = File.ReadAllText(filePath);

                    // Deserialize the JSON data back to a list of users
                    return JsonConvert.DeserializeObject<List<User>>(jsonData);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during file loading
                // You can log the error or take appropriate action
            }

            return new List<User>(); // Return an empty list if the file doesn't exist or there's an error
        }

        public void handleMessage(ChatGroup chatGroup,ChatMessage newMessage)
        {
            foreach (ChatGroup c in chatGroups)
            {
                if (c.Name.Equals(chatGroup.Name))
                {
                   c.ChatHistory.Add(newMessage);
                }

            }


            SaveChatGroups();
        }

        public List<ChatMessage> LoadChatHistory(ChatGroup chatGroup)
        {

            // Check if the chat group exists in your chatGroups list
            var existingChatGroup = chatGroups.FirstOrDefault(c => c.Name == chatGroup.Name);

            if (existingChatGroup != null)
            {
                // Return the chat history from the existing ChatGroup object
                return existingChatGroup.ChatHistory;
            }
            else
            {
                // If the chat group doesn't exist, return an empty list
                return new List<ChatMessage>();
            }
        }

        public void handleLeaveGroup(ChatGroup chatgroup, User user)
        {
            foreach (ChatGroup c in chatGroups)
            {
                if (c.Name.Equals(chatgroup.Name))
                {
                    c.Members.Remove(user);
                }

            }

            foreach (User u in users)
            {
                if (u.Username.Equals(user.Username))
                {
                    u.JoinedGroups.Remove(chatgroup);
                }

            }

            SaveChatGroups();
            SaveUserData();

        }

        public List<User> LoadChatGroupMembers(ChatGroup chatgroup)
        {
            foreach (ChatGroup c in chatGroups)
            {
                if (c.Name.Equals(chatgroup.Name))
                {
                    return c.Members;
                }

            }
            return new List<User>();
        }

     /*
    public List<ChatGroup> GetAllGroupChats()
        {
            return chatGroups;
        }

        public List<string> GetUsersInChat(string chatName)
        {
            
            // Find the chat group with the given name
            var chatGroup = chatGroups.FirstOrDefault(c => c.Name == chatName);

            if (chatGroup != null)
            {
                return chatGroup.Members;
            }
            
            return new List<string>(); // Return an empty list if the chat group is not found
        }
     */
     
    }
}
