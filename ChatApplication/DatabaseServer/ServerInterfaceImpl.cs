using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UserDLL;

namespace DatabaseServer
{
    public class ServerInterfaceImpl : ServerInterface
    {
        private List<User> users = new List<User>();
        private List<ChatGroup> chatGroups = new List<ChatGroup>();

        public ServerInterfaceImpl()
        {
            users = LoadUserData();
            chatGroups = LoadChatGroups();
        }

        //handling user registration
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

        //handling user login
        public User LoginUser(string username)
        {
            User user = users.FirstOrDefault(u => u.Username == username); //Check if username is found
            if (user != null)
            { return user; } //return true if user found
            return null;
        }

        //handling user creating a new group
        public bool CreateGroupChat(ChatGroup chatGroup, User user)
        {
            // Check if the chat group name is unique
            if (chatGroups.Any(c => string.Equals(c.Name, chatGroup.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return false; // Chat group name already exists
            }
            else
            {
                chatGroup.Members.Add(user);
                // Update the JoinedGroups list for all users
                foreach (User u in users)
                {
                    if (u.Username.Equals(user.Username))
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
        }


        //Handling user join group
        public bool JoinGroupChat(ChatGroup chatGroup, User user)
        {
            bool isUserAlreadyMember = user.JoinedGroups.Any(group => group.Name == chatGroup.Name);
            if (!isUserAlreadyMember)
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
            }

            return false; // Chat group not found
        }

       // find user method
        public User GetUser(String memberUsername)
        {
            foreach (User u in users)
            {
                if (u.Username.Equals(memberUsername))
                {
                    return u;
                }
            }
            return new User();
        }

        // Save chat groups to a JSON file with compressed data
        public void SaveChatGroups()
        {
            try
            {
                string filePath = "chatGroups.json";

                // Create a list of chat groups with compressed data
                List<ChatGroup> chatGroupsToSave = new List<ChatGroup>();

                foreach (ChatGroup chatGroup in chatGroups)
                {
                    // Create a new chat group with compressed chat history
                    ChatGroup chatGroupToSave = new ChatGroup
                    {
                        Name = chatGroup.Name,
                        Members = chatGroup.Members,
                        ChatHistory = new List<ChatMessage>()
                    };

                    foreach (ChatMessage message in chatGroup.ChatHistory)
                    {
                        // Compress each attachment in the message
                        ChatMessage compressedMessage = new ChatMessage
                        {
                            Sender = message.Sender,
                            Timestamp = message.Timestamp,
                            Message = message.Message,
                            Attachments = message.Attachments.Select(attachment =>
                            {
                                // Compress the attachment data
                                byte[] compressedData = CompressData(attachment.FileData);
                                attachment.FileData = compressedData;
                                return attachment;
                            }).ToList()
                        };

                        chatGroupToSave.ChatHistory.Add(compressedMessage);
                    }

                    chatGroupsToSave.Add(chatGroupToSave);
                }
                string jsonData = JsonConvert.SerializeObject(chatGroupsToSave, Formatting.Indented);
                File.WriteAllText(filePath, jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving group chats details : ", ex.ToString());
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
                Console.WriteLine("Error loading group chats details : ", ex.ToString());
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
                Console.WriteLine("Error saving user data: " + ex.Message);
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
                Console.WriteLine("Error loading user data: " + ex.Message);
            }

            return new List<User>(); // Return an empty list if the file doesn't exist or there's an error
        }

        //handling user send message
        public void handleMessage(ChatGroup chatGroup, ChatMessage newMessage)
        {
            foreach (ChatGroup c in chatGroups)
            {
                if (c.Name.Equals(chatGroup.Name))
                {
                    foreach (FileAttachment attachment in newMessage.Attachments)
                    {
                        if (attachment.isCompressed)
                        {
                            // Decompress the compressed data
                            attachment.FileData = DecompressData(attachment.FileData);
                        }
                    }

                    // Add the message to the chat history
                    c.ChatHistory.Add(newMessage);
                }
            }

            SaveChatGroups();
        }


        // Compress the file data
        public byte[] CompressData(byte[] data)
        {
            using (MemoryStream output = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream(output, CompressionMode.Compress))
                {
                    gzipStream.Write(data, 0, data.Length);
                }
                return output.ToArray();
            }
        }

        // Decompress the compressed data
        public byte[] DecompressData(byte[] compressedData)
        {
            using (MemoryStream input = new MemoryStream(compressedData))
            using (GZipStream gzipStream = new GZipStream(input, CompressionMode.Decompress))
            using (MemoryStream output = new MemoryStream())
            {
                byte[] buffer = new byte[4096];
                int bytesRead;
                while ((bytesRead = gzipStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    output.Write(buffer, 0, bytesRead);
                }
                return output.ToArray();
            }
        }

        //handling private message 
        public void handlePrivateMessage(User messagingUser, User receivingUser, PrivateMessage newMessage)
        {

            foreach (User user in users)
            {
                if (user.Username.Equals(messagingUser.Username))
                {
                    user.PrivateMessages.Add(newMessage);
                }
                //adds the receiving user information to the messaging user

                if (user.Username.Equals(receivingUser.Username))
                {
                    user.PrivateMessages.Add(newMessage);
                }
                //adds the messaging user's name to the receivings users list
            }
            SaveUserData();
        }

        //handling loading group chat history
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

        //handling loading private message chat history
        public List<PrivateMessage> LoadPMHistory(User messagingUser)
        {
            // Check if the chat group exists in your chatGroups list
            var existingUser = users.FirstOrDefault(c => c.Username == messagingUser.Username);

            if(existingUser != null) 
            {
                return existingUser.PrivateMessages;
            }        
            else
            {
                // If the private message doesn't exist, return an empty list
                return new List<PrivateMessage>(); 
            }

        }

        //handling user leaving group
        public bool handleLeaveGroup(ChatGroup chatgroup, User user)
        {
            if (chatgroup != null && user != null)
            {
                foreach (ChatGroup c in chatGroups)
                {
                    if (c.Name.Equals(chatgroup.Name))
                    {
                        // Use a separate list to collect members for removal
                        List<User> usersToRemove = new List<User>();

                        foreach (User u in c.Members)
                        {
                            if (u.Username.Equals(user.Username))
                            {
                                // Add the user to the removal list
                                usersToRemove.Add(u);
                            }
                        }

                        // Remove the collected users from the group
                        foreach (User u in usersToRemove)
                        {
                            c.Members.Remove(u);
                        }



                    }
                }
                foreach (User u in users)
                {
                    if (u.Username.Equals(user.Username))
                    {
                        // Use a separate list to collect members for removal
                        List<ChatGroup> groupToRemove = new List<ChatGroup>();

                        foreach (ChatGroup c in u.JoinedGroups)
                        {
                            if (c.Name.Equals(chatgroup.Name))
                            {
                                // Add the user to the removal list
                                groupToRemove.Add(c);
                            }
                        }

                        // Remove the collected users from the group
                        foreach (ChatGroup c in groupToRemove)
                        {
                            u.JoinedGroups.Remove(c);
                        }



                    }
                }
                // Save the modified chat groups list to the file
                SaveChatGroups();

                // Save the modified user list to the file
                SaveUserData();
                return true;

            }
            else
            {
                Console.WriteLine("Unable to find user from the group ");
            }

            return false;
        }

        //handling loading members from chat group
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

    }
}
