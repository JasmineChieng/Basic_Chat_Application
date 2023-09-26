using BusinessTierServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UserDLL;

namespace ChatApp
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    /// 

    public partial class Window1 : Window
    {
        private Page3 page3;
        private User user;
        private BusinessInterface foob;
        public string ButtonContentForChatHistory { get; set; }
        public Window1(BusinessInterface foob, User user)
        {
            InitializeComponent();
            this.user = user;
            this.foob = foob;
            // Navigate the Frame to the default page when the window loads
            Page2 defaultPage = new Page2(); // Replace with the actual default page
            ChatBox.NavigationService.Navigate(defaultPage);

            page3 = new Page3(this, user, foob);
            refreshChatContainer(user);

        }

        public void refreshChatContainer(User user)
        {

            ChatContainer.Children.Clear();
            List<ChatGroup> groupList = user.JoinedGroups;
            if (groupList != null)
            {
                foreach (var content in groupList)
                {
                    // Use the GroupButton_UI method to create buttons
                    Button groupButton = page3.GroupButton_UI(content);

                    groupButton.Click += Page3_GroupButton_Click; // Handle button click event

                    // Add the loaded button to the ChatContainer
                    ChatContainer.Children.Add(groupButton);
                }
            }
        }
        private void createGroupBtn_Click(object sender, RoutedEventArgs e)
        {
            ChatBox.Content = new Page3(this, user, foob);
        }

        private void viewGroupBtn_Click(object sender, RoutedEventArgs e)
        {
            page3 = new Page3(this, user, foob);
            ChatBox.Content = new Page4(page3, user, foob, this);
        }

        private void Page3_GroupButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            // Retrieve the ChatGroup object from the Tag property
            if (clickedButton.Tag is ChatGroup selectedChatGroup)
            {
                Page1 chatHistoryPage = new Page1(this, user, selectedChatGroup, foob);
                ChatBox.NavigationService.Navigate(chatHistoryPage);
            }
        }


        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow login = new MainWindow();
            this.Visibility = Visibility.Hidden;
            login.Show();
        }
    }


}
