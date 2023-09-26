using System;
using System.Collections.Generic;
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

        public string ButtonContentForChatHistory { get; set; }
        public Window1()
        {
            InitializeComponent();
            // Navigate the Frame to the default page when the window loads
            Page2 defaultPage = new Page2(); // Replace with the actual default page
            ChatBox.NavigationService.Navigate(defaultPage);

            page3 = new Page3(this);
            // Load the saved button content list from Page3
            List<ChatGroup> groupList = page3.GroupList;
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
            ChatBox.Content = new Page3(this);
        }

        private void viewGroupBtn_Click(object sender, RoutedEventArgs e)
        {
            page3 = new Page3(this);
            ChatBox.Content = new Page4(page3.GroupList);
        }

        private void Page3_GroupButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            string groupName = GetTextBlockContent(clickedButton);

            // Set the ButtonContent property before navigating to ChatHistoryPage
            ButtonContentForChatHistory = groupName;

            // Create an instance of the ChatHistoryPage and navigate to it
            Page1 chatHistoryPage = new Page1();
            chatHistoryPage.SetChatHistory(groupName); // Pass the group name to set chat history
            ChatBox.NavigationService.Navigate(chatHistoryPage);
        }

        private string GetTextBlockContent(Button button)
        {
            // Assuming that the TextBlock is the first child of the Grid within the Button's Content
            if (button.Content is StackPanel panel)
            {
                if (panel.Children.Count > 0 && panel.Children[1] is TextBlock textBlock)
                {
                    return textBlock.Text;
                }
            }

            return string.Empty; // Return an empty string if not found
        }

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow login = new MainWindow();
            this.Visibility = Visibility.Hidden;
            login.Show();
        }
    }

    
}
