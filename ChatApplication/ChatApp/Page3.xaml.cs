using BusinessTierServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Page3.xaml
    /// </summary>
    public partial class Page3 : Page
    {
        private Window1 mainWindow;
        private List<ChatGroup> groupsList = new List<ChatGroup>();
        private Page4 page4;
        private User user;
        private BusinessInterface foob;

        public Page3(Window1 window, User user, BusinessInterface foob)
        {
            InitializeComponent();
            this.mainWindow = window;
            this.foob = foob;
            // Load data file
            groupsList = foob.LoadChatGroups();
            // Initialize Page4 and pass the groupsList
            page4 = new Page4(this, user, foob, mainWindow);
            this.user = user;
        }

        //If private checkbox is checked, then ask user to enter a join code
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            joinCodeLabel.Visibility = Visibility.Visible;
            joinCodeTB.Visibility = Visibility.Visible;
        }

        //If is not private, hide the join code
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            joinCodeLabel.Visibility = Visibility.Hidden;
            joinCodeTB.Visibility = Visibility.Hidden;
        }

        //Handling create button clicked event
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string newGroupName = groupNameTB.Text;
            bool isPrivate = checkBox.IsChecked ?? false; // Check the CheckBox value
            string joinCode = joinCodeTB.Text;
            // Create a new group and add it to the shared list
            var newGroup = new ChatGroup { Name = newGroupName, IsPrivate = isPrivate };

            // If the group is private and a join code is provided, set the join code
            if (isPrivate)
            {
                if (!string.IsNullOrWhiteSpace(joinCode))
                {
                    newGroup.JoinCode = joinCode;
                }
                else
                {
                    MessageBox.Show("Join Code cannot be empty");
                }

            }

            //Check is group name already exist
            if (foob.CreateGroupChat(newGroup, user))
            {
                groupsList.Add(newGroup);



                Button groupButton = GroupButton_UI(newGroup);
                if (mainWindow != null)
                {
                    StackPanel chatContainer = mainWindow.ChatContainer;
                    chatContainer.Children.Add(groupButton);
                }

                groupButton.Click += (s, args) =>
                {
                    Button clickedButton = (Button)s;
                    string groupName = clickedButton.Content.ToString();

                    Page1 chatHistoryPage = new Page1(mainWindow, user, newGroup, foob);
                    mainWindow.ChatBox.NavigationService.Navigate(chatHistoryPage);
                };

                MessageBox.Show($"Successfully created new group: {newGroupName}");
            }
            else
            {
                MessageBox.Show($"Unable to create new group because group Name already exists");
            }
        }

        private void GroupButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            string groupName = clickedButton.Content.ToString();

        }


        //public method to return groupList
        public List<ChatGroup> GroupList
        {
            get { return groupsList; }
        }

        
        public Button GroupButton_UI(ChatGroup group)
        {
            // Create a StackPanel to hold the text and image
            StackPanel stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string imageRelativePath = "groupsLogo.png";
            string imagePath = System.IO.Path.Combine(baseDirectory, imageRelativePath);


            // Create an Image control and set its properties
            Image image = new Image
            {
                Source = new BitmapImage(new Uri(imagePath)),
                Width = 24,
                Height = 24,
                Margin = new Thickness(0, 0, 5, 0) 
            };

            // Create a TextBlock to display the text
            TextBlock textBlock = new TextBlock
            {
                Text = group.Name, // Use the group name from the ChatGroup object
                VerticalAlignment = VerticalAlignment.Center,
                Width = 100,
                FontSize = 14
            };

            // Add the Image and TextBlock to the StackPanel
            stackPanel.Children.Add(image);
            stackPanel.Children.Add(textBlock);

            // Set the StackPanel as the Content of the button
            Button groupButton = new Button
            {
                Content = stackPanel,
                Height = 30,
                Width = 150,
                Background = Brushes.Transparent,
                Foreground = Brushes.Black,
                FontSize = 15,
            };

            // Set the Tag property of the button to the ChatGroup object
            groupButton.Tag = group;

            groupButton.Click += GroupButton_Click; // Handle button click event
            return groupButton;
        }



    }
}
