using BusinessTierServer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserDLL;

namespace ChatApp
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {

        private ObservableCollection<ChatMessage> chatMessages = new ObservableCollection<ChatMessage>();
        private User user;
        private ChatGroup chatgroup;
        BusinessInterface foob;
        Window1 chatWindow;

        // for PM Constructor user
        private User receivingUser;
        private ObservableCollection<PrivateMessage> privateMessagesCollection = new ObservableCollection<PrivateMessage>();
        private HashSet<string> createdButtons = new HashSet<string>(); // Create a HashSet to track created buttons
        private bool isPrivate=false;


        public Page1(Window1 chatWindow, User user, ChatGroup chatgroup, BusinessInterface foob)
        {
            InitializeComponent();
            chatListBox.ItemsSource = chatMessages;
            this.user = user;
            this.chatgroup = chatgroup;
            this.foob = foob;
            this.chatWindow = chatWindow;

            // Load chat history for the specified ChatGroup
            List<ChatMessage> chatHistory = foob.LoadChatHistory(chatgroup);

            // Add the loaded chat history messages to the chatMessages ObservableCollection
            foreach (var message in chatHistory)
            {
                chatMessages.Add(message);
            }

            // Load the chat group's members first
            LoadChatGroupMembers();

            // Call the method to populate the sidePanel with member buttons
            PopulateSidePanelWithMemberButtons();

            isPrivate = false;
        }

        //Overloaded constructor to create second type of page 1, for private messaging
        public Page1(User messagingUser, User receivingUser, BusinessInterface foob)
        {
            InitializeComponent();

            List<PrivateMessage> tempPMList = foob.LoadPMHistory(messagingUser);
            //List<PrivateMessage> tempPMList = messagingUser.PrivateMessages;
            //List of private messages that the current user has sent before

            List<PrivateMessage> privateChatHistory = new List<PrivateMessage>();
            //List of private messages that is between the messaging and receiving user

            bool isTempEmpty = !tempPMList.Any();
            if (!isTempEmpty)
            {//if there are existing messages
                foreach (PrivateMessage pm in tempPMList)
                {
                    //for each message sent in the message list of current user, we do:
                    if ((pm.Receiver != null) && pm.Sender != null)
                    {
                        if (pm.Receiver.Equals(receivingUser.Username) || (pm.Sender.Equals(receivingUser.Username)))
                        {
                            //if the name of the receiver in the pm is the same as the name of the receiving user object we receive, do:
                            privateChatHistory.Add(pm);
                            //add the pm to privateChat history
                        }
                    }
                }
            }

            chatListBox.ItemsSource = privateMessagesCollection; //set item source to the view only collection
            this.user = messagingUser;
            this.receivingUser = receivingUser;
            this.foob = foob;

            // Add the filtered messages to the ObservableCollection
            bool isHistoryEmpty = !privateChatHistory.Any();
            if (!isHistoryEmpty)
            {
                foreach (var message in privateChatHistory)
                {
                    privateMessagesCollection.Add(message);
                }
            }

            viewMoreBtn.Visibility = Visibility.Collapsed;

            // Load the chat group's members first
            //LoadChatGroupMembers();

            // Call the method to populate the sidePanel with member buttons
            //PopulateSidePanelWithMemberButtons();

            sendBtn.Click -= sendBtn_Click;
            sendBtn.Click += sendPMBtn_Click;

            isPrivate = true;
        }


        //For creating private chat button in window1
        public Page1(Window1 chatWindow, User user, BusinessInterface foob)
        {
            InitializeComponent();
            chatListBox.ItemsSource = chatMessages;
            this.user = user;
            this.foob = foob;
            this.chatWindow = chatWindow;

        }
        private void LoadChatGroupMembers()
        {
            List<User> members = foob.LoadChatGroupMembers(chatgroup);

            // Update the chat group's members with the loaded members
            chatgroup.Members.Clear();
            chatgroup.Members.AddRange(members);
        }


        
        private void sendBtn_Click(object sender, RoutedEventArgs e)
        {
            
            // Create a new chat message when the Send button is clicked
            string userMessage = messageTB.Text;

            if (!string.IsNullOrWhiteSpace(userMessage))
            {
                ChatMessage newMessage = new ChatMessage
                {
                    Sender = user.Username,
                    Message = userMessage,
                    Timestamp = DateTime.Now
                };


                chatMessages.Add(newMessage);
                messageTB.Clear(); // Clear the message input field

                foob.handleMessage(chatgroup, newMessage);
            }
        }

      
        //same as normal send, but for PM
        private void sendPMBtn_Click(object sender, RoutedEventArgs e)
        {
            
            // Create a new chat message when the Send button is clicked
            string userMessage = messageTB.Text;

            if (!string.IsNullOrWhiteSpace(userMessage))
            {
                PrivateMessage newMessage = new PrivateMessage
                {
                    Sender = user.Username,
                    Receiver = receivingUser.Username,
                    Message = userMessage,
                    Timestamp = DateTime.Now
                };

                privateMessagesCollection.Add(newMessage);
                messageTB.Clear(); // Clear the message input field

                foob.handlePrivateMessage(user, receivingUser, newMessage);
            }


        }

        private void viewMoreBtn_Click(object sender, RoutedEventArgs e)
        {
            // Show the side panel
            sidePanel.Visibility = Visibility.Visible;
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            // Hide the side panel when the close button is clicked
            sidePanel.Visibility = Visibility.Collapsed;

        }

        private void PopulateSidePanelWithMemberButtons()
        {
            // Clear the existing buttons in the sideStackPanel
            sideStackPanel.Children.Clear();

            foreach (User member in chatgroup.Members)
            {
                // Create a button for each member
                Button memberButton = new Button
                {
                    Content = member.Username, // Display the member's username on the button
                    Width = 150,
                    Height = 30,
                    Margin = new Thickness(10, 5, 10, 5),
                    Background = new SolidColorBrush(Colors.Yellow) // Set the background color
                };

                // Handle the click event for the member button
                memberButton.Click += MemberButton_Click;

                // Add the member button to the sideStackPanel
                sideStackPanel.Children.Add(memberButton);
            }
        }



        private void MemberButton_Click(object sender, RoutedEventArgs e)
        {
            List<PrivateMessage> privateMessages = user.PrivateMessages;


            Button clickedButton = (Button)sender;
            string memberUsername = clickedButton.Content.ToString();

            if (!memberUsername.Equals(user.Username) && !createdButtons.Contains(memberUsername))
            {
             
                        User receivingUser = foob.GetUser(memberUsername);
                        if (receivingUser != null)
                        {
                 
                            if (!user.PrivateMessages.Any(pm => pm.Sender == receivingUser.Username))
                            {
                                Button groupButton = PrivateButton_UI(receivingUser);
                                if (chatWindow != null)
                                {
                                    StackPanel chatContainer = chatWindow.ChatContainer;
                                    chatContainer.Children.Add(groupButton);
                                }

                                groupButton.Click += (s, args) =>
                                {
                                    Button clickedButtonz = (Button)s;
                                    string username = clickedButtonz.Content.ToString();

                                    Page1 memberPage = new Page1(user, receivingUser, foob);
                                    chatWindow.ChatBox.NavigationService.Navigate(memberPage);
                                };

                                createdButtons.Add(memberUsername); // Add the username to the HashSet to mark it as created
                            }
                        }

                    
                
                    
            }
        }

        private void leaveGroupBtn_Click(object sender, RoutedEventArgs e)
        {
            if (foob.handleLeaveGroup(chatgroup, user))
            {
                StackPanel chatContainer = chatWindow.ChatContainer;
                string groupNameToRemove = chatgroup.Name; // Replace with the actual group name

                // Find and remove the child Button with the matching group name
                Button buttonToRemove = null;

                foreach (UIElement child in chatContainer.Children)
                {
                    if (child is Button button && button.Tag is ChatGroup group)
                    {
                        if (group.Name == groupNameToRemove)
                        {
                            buttonToRemove = button; MessageBox.Show("Leave group .");
                            break; // Stop searching after finding the matching group name

                        }
                    }
                }

                // Remove the button from the chatContainer if it was found
                if (buttonToRemove != null)
                {
                    chatContainer.Children.Remove(buttonToRemove);
                }

                // Update the chat group's members after the user has left
                LoadChatGroupMembers();

                // Update the side panel with member buttons
                PopulateSidePanelWithMemberButtons();

                Page2 defaultPage = new Page2();
                chatWindow.ChatBox.NavigationService.Navigate(defaultPage);

            }
            else
            {
                MessageBox.Show("Fail to leave group.");
            }

        }
     
        private void sendFileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png, *.gif, *.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp|Text Files (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true)
            {
                // User selected a file, handle it here
                string selectedFilePath = openFileDialog.FileName;
                string selectedFileExtension = System.IO.Path.GetExtension(selectedFilePath).ToLower();

                if (isPrivate)
                {
                    // Handle private message attachment
                    HandlePrivateMessageAttachment(selectedFilePath, selectedFileExtension, receivingUser);
                }
                else
                {
                    // Handle chat message attachment
                    HandleChatMessageAttachment(selectedFilePath, selectedFileExtension);
                }
            }
        }

        private void HandlePrivateMessageAttachment(string selectedFilePath, string selectedFileExtension, User receiver)
        {
            // Create a private message
            PrivateMessage privateMessage = new PrivateMessage
            {
                Sender = user.Username,
                Receiver = receiver.Username,
                Timestamp = DateTime.Now
            };

            if (selectedFileExtension == ".txt")
            {
                // Handle text file upload
                byte[] fileData = File.ReadAllBytes(selectedFilePath);
                privateMessage.Message = $"[File: {System.IO.Path.GetFileName(selectedFilePath)}]";

                // Attach the text file as a file attachment
                privateMessage.Attachments.Add(new FileAttachment
                {
                    FileName = System.IO.Path.GetFileName(selectedFilePath),
                    FileData = fileData
                });
            }
            else if (selectedFileExtension == ".jpg" ||
                     selectedFileExtension == ".jpeg" ||
                     selectedFileExtension == ".png" ||
                     selectedFileExtension == ".gif" ||
                     selectedFileExtension == ".bmp")
            {
                // Handle image file upload
                byte[] imageData = File.ReadAllBytes(selectedFilePath);
                privateMessage.Message = $"[Image: {System.IO.Path.GetFileName(selectedFilePath)}]";

                // Attach the image as a file attachment
                privateMessage.Attachments.Add(new FileAttachment
                {
                    FileName = System.IO.Path.GetFileName(selectedFilePath),
                    FileData = imageData,
                    ImageFormat = selectedFileExtension.Substring(1) // Store the image format as a string
                });
            }

            // Add the private message to the collection and handle it
            user.PrivateMessages.Add(privateMessage);
            receiver.PrivateMessages.Add(privateMessage);
            privateMessagesCollection.Add(privateMessage);
            foob.handlePrivateMessage(user,receiver,privateMessage);
        }

        private void HandleChatMessageAttachment(string selectedFilePath, string selectedFileExtension)
        {
            // Create a chat message
            ChatMessage chatMessage = new ChatMessage
            {
                Sender = user.Username,
                Timestamp = DateTime.Now
            };

            if (selectedFileExtension == ".txt")
            {
                // Handle text file upload
                byte[] fileData = File.ReadAllBytes(selectedFilePath);
                chatMessage.Message = $"[File: {System.IO.Path.GetFileName(selectedFilePath)}]";

                // Attach the text file as a file attachment
                chatMessage.Attachments.Add(new FileAttachment
                {
                    FileName = System.IO.Path.GetFileName(selectedFilePath),
                    FileData = fileData
                });
            }
            else if (selectedFileExtension == ".jpg" ||
                     selectedFileExtension == ".jpeg" ||
                     selectedFileExtension == ".png" ||
                     selectedFileExtension == ".gif" ||
                     selectedFileExtension == ".bmp")
            {
                // Handle image file upload
                byte[] imageData = File.ReadAllBytes(selectedFilePath);
                chatMessage.Message = $"[Image: {System.IO.Path.GetFileName(selectedFilePath)}]";

                // Attach the image as a file attachment
                chatMessage.Attachments.Add(new FileAttachment
                {
                    FileName = System.IO.Path.GetFileName(selectedFilePath),
                    FileData = imageData,
                    ImageFormat = selectedFileExtension.Substring(1) // Store the image format as a string
                });
            }

            // Add the chat message to the collection and handle it
            chatMessages.Add(chatMessage);
            foob.handleMessage(chatgroup, chatMessage);
        }
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;
           

            if (isPrivate)
            {
                PrivateMessage privateMessage = (PrivateMessage)textBlock.DataContext;

                if (PHasFileAttachment(privateMessage))
                {
                    // Trigger file download here
                    PDownloadFileAttachment(privateMessage);
                }
            }
            else
            {
                ChatMessage clickedMessage = (ChatMessage)textBlock.DataContext;


                if (HasFileAttachment(clickedMessage))
                {
                    // Trigger file download here
                    DownloadFileAttachment(clickedMessage);
                }

            }
        }

        private bool HasFileAttachment(ChatMessage message)
        {
            // Check if the message has any attachments
            return message.Attachments != null && message.Attachments.Count > 0;
        }

        private void DownloadFileAttachment(ChatMessage message)
        {
            // Prompt the user to select a download location
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = message.Attachments[0].FileName; // Set the default file name
            if (saveFileDialog.ShowDialog() == true)
            {
                string downloadPath = saveFileDialog.FileName;

                // Check the type of attachment and save it to the specified location
                foreach (FileAttachment attachment in message.Attachments)
                {
                    if (attachment is FileAttachment txtAttachment)
                    {
                        // Handle text file attachment download
                        File.WriteAllBytes(downloadPath, txtAttachment.FileData);
                    }
                    else if (attachment is FileAttachment imgAttachment)
                    {
                        // Handle image file attachment download
                        File.WriteAllBytes(downloadPath, imgAttachment.FileData);
                    }
                }

                MessageBox.Show($"File downloaded to: {downloadPath}");
            }
        }


        private bool PHasFileAttachment(PrivateMessage message)
        {
            // Check if the message has any attachments
            return message.Attachments != null && message.Attachments.Count > 0;
        }

        private void PDownloadFileAttachment(PrivateMessage message)
        {
            // Prompt the user to select a download location
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = message.Attachments[0].FileName; // Set the default file name
            if (saveFileDialog.ShowDialog() == true)
            {
                string downloadPath = saveFileDialog.FileName;

                // Check the type of attachment and save it to the specified location
                foreach (FileAttachment attachment in message.Attachments)
                {
                    if (attachment is FileAttachment txtAttachment)
                    {
                        // Handle text file attachment download
                        File.WriteAllBytes(downloadPath, txtAttachment.FileData);
                    }
                    else if (attachment is FileAttachment imgAttachment)
                    {
                        // Handle image file attachment download
                        File.WriteAllBytes(downloadPath, imgAttachment.FileData);
                    }
                }

                MessageBox.Show($"File downloaded to: {downloadPath}");
            }
        }
        public Button PrivateButton_UI(User user)
        {
            // Create a StackPanel to hold the text and image
            StackPanel stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string imageRelativePath = "personLogo.png";
            string imagePath = System.IO.Path.Combine(baseDirectory, imageRelativePath);


            // Create an Image control and set its properties
            Image image = new Image
            {
                Source = new BitmapImage(new Uri(imagePath)), // Replace 'icon.png' with your image path
                Width = 24,
                Height = 24,
                Margin = new Thickness(0, 0, 5, 0) // Optional margin to separate image and text
            };

            // Create a TextBlock to display the text
            TextBlock textBlock = new TextBlock
            {
                Text = user.Username, // Use the group name from the ChatGroup object
                VerticalAlignment = VerticalAlignment.Center,
                Width = 100,
                FontSize = 14
            };

            // Add the Image and TextBlock to the StackPanel
            stackPanel.Children.Add(image);
            stackPanel.Children.Add(textBlock);

            // Set the StackPanel as the Content of the button
            Button privateButton = new Button
            {
                Content = stackPanel,
                Height = 30,
                Width = 150,
                Background = Brushes.Transparent,
                Foreground = Brushes.Black,
                FontSize = 15,
                // Add other customizations as needed
            };

            // Set the Tag property of the button to the ChatGroup object
            privateButton.Tag = user;

            privateButton.Click += PrivateButton_Click; // Handle button click event
            return privateButton;
        }


        public void PrivateButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            string username = clickedButton.Content.ToString();

        }

    }



}
