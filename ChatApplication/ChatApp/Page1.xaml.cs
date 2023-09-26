using BusinessTierServer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private User user; // messaging user
        private ChatGroup chatgroup;
        BusinessInterface foob;

        // for second constructor user
        private User receivingUser;
        private ObservableCollection<PrivateMessage> privateMessagesCollection = new ObservableCollection<PrivateMessage>();

        public Page1(User user,ChatGroup chatgroup,BusinessInterface foob)
        {
            InitializeComponent();
            chatListBox.ItemsSource = chatMessages;
            this.user = user;
            this.chatgroup = chatgroup;
            this.foob = foob;

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

        //Overloaded constructor to create second type of page 1, for private messaging
        public Page1(User messagingUser, User receivingUser, BusinessInterface foob)
        {
            InitializeComponent();

            List<PrivateMessage> tempPMList = messagingUser.PrivateMessages;
            //List of private messages that the current user has sent before
            List<PrivateMessage> privateChatHistory = new List<PrivateMessage>();
            //List of private messages that is between the messaging and receiving user

            bool isTempEmpty = !tempPMList.Any();
            if (!isTempEmpty)
            {//if there are existing messages
                foreach (PrivateMessage pm in tempPMList)
                {
                    //for each message sent in the message list of current user, we do:
                    if (pm.ReceiverName != null)
                    {
                        if (pm.ReceiverName.Equals(receivingUser.Username))
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
            // Load the chat group's members first
            //LoadChatGroupMembers();

            // Call the method to populate the sidePanel with member buttons
            //PopulateSidePanelWithMemberButtons();

            sendBtn.Click -= sendBtn_Click;
            sendBtn.Click += sendPMBtn_Click;
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
                    Message = userMessage
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
                    ReceiverName = receivingUser.Username,
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
            Button clickedButton = (Button)sender;
            string memberUsername = clickedButton.Content.ToString();

            if (!memberUsername.Equals(user.Username)) //only run if the user we are looking at is not the the current user
            {
                User receivingUser = foob.getUser(memberUsername); //retrieve the user from the user list
                if (receivingUser != null) //if it is not empty (meaning we managed to retrieve it)
                {
                    Page1 memberPage = new Page1(user, receivingUser, foob);
                    //if (foob.StartPrivateChat(user, receivingUser))
                    //{
                        //change this to start or add
                        NavigationService.Navigate(memberPage);
                    //}
                }
            }

            
            //ChatBox.NavigationService.Navigate(chatHistoryPage);
            /*
            if (clickedButton.Tag is ChatGroup selectedChatGroup)
            {
                Page1 chatHistoryPage = new Page1(user, selectedChatGroup, foob);
                ChatBox.NavigationService.Navigate(chatHistoryPage);
            }
            */
        }

        private void leaveGroupBtn_Click(object sender, RoutedEventArgs e)
        {
            foob.handleLeaveGroup(chatgroup, user);
        }
    }



}
