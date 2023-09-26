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
            Button clickedButton = (Button)sender;
            string memberUsername = clickedButton.Content.ToString();
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

                if (selectedFileExtension == ".txt")
                {
                    // Handle text file upload
                    byte[] fileData = File.ReadAllBytes(selectedFilePath);

                    // Create a chat message with the text file as an attachment
                    ChatMessage newMessage = new ChatMessage
                    {
                        Sender = user.Username, // Replace with the sender's username
                        Timestamp = DateTime.Now,
                        Message = $"[File: {System.IO.Path.GetFileName(selectedFilePath)}]"
                    };

                    // Attach the text file as a file attachment
                    newMessage.Attachments.Add(new FileAttachment
                    {
                        FileName = System.IO.Path.GetFileName(selectedFilePath),
                        FileData = fileData
                    });

                    chatMessages.Add(newMessage);

                    foob.handleMessage(chatgroup, newMessage);
                }
                else if (selectedFileExtension == ".jpg" ||
                         selectedFileExtension == ".jpeg" ||
                         selectedFileExtension == ".png" ||
                         selectedFileExtension == ".gif" ||
                         selectedFileExtension == ".bmp")
                {
                    // Handle image file upload
                    byte[] imageData = File.ReadAllBytes(selectedFilePath);

                    // Create a chat message with the image attachment
                    ChatMessage newMessage = new ChatMessage
                    {
                        Sender = user.Username, // Replace with the sender's username
                        Timestamp = DateTime.Now,
                        Message = $"[Image: {System.IO.Path.GetFileName(selectedFilePath)}]", // Display as a link
                    };

                    // Attach the image as a file attachment
                    newMessage.Attachments.Add(new FileAttachment
                    {
                        FileName = System.IO.Path.GetFileName(selectedFilePath),
                        FileData = imageData,
                        ImageFormat = selectedFileExtension.Substring(1) // Store the image format as a string
                    });

                    chatMessages.Add(newMessage);

                    foob.handleMessage(chatgroup, newMessage);
                }
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;
            ChatMessage clickedMessage = (ChatMessage)textBlock.DataContext;

            if (HasFileAttachment(clickedMessage))
            {
                // Trigger file download here
                DownloadFileAttachment(clickedMessage);
            }
        }

        private bool HasFileAttachment(ChatMessage message)
        {
            // Check if the message has any attachments
            return message.Attachments != null && message.Attachments.Count > 0;
        }

        /*
        private void DownloadFileAttachment(ChatMessage message)
        {
            // Handle the file attachment download here
            foreach (FileAttachment attachment in message.Attachments)
            {
                // Check the type of attachment and take appropriate action
                if (attachment is FileAttachment txtAttachment)
                {
                    // Handle text file attachment download
                    // For example, you can display it in a dialog or save it to disk
                    MessageBox.Show($"Downloading text file: {txtAttachment.FileName}");
                }
                else if (attachment is FileAttachment imgAttachment)
                {
                    // Handle image file attachment download
                    // For example, you can display it in an image viewer or save it to disk
                    MessageBox.Show($"Downloading image: {imgAttachment.FileName}");
                }
            }
        }*/

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
    }



    }
