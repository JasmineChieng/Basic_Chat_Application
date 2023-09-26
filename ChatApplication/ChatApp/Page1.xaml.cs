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
        private User user;
        private ChatGroup chatgroup;
        BusinessInterface foob;

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
            foob.handleLeaveGroup(chatgroup, user);
        }
    }



}
