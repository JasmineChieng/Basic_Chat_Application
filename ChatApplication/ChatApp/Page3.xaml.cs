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

        public Page3(Window1 window)
        {
            InitializeComponent();
            this.mainWindow = window;
            // Load data file
            LoadGroupListFromFile();

            // Initialize Page4 and pass the groupsList
            page4 = new Page4(groupsList);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            joinCodeLabel.Visibility = Visibility.Visible;
            joinCodeTB.Visibility = Visibility.Visible;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            joinCodeLabel.Visibility = Visibility.Hidden;
            joinCodeTB.Visibility = Visibility.Hidden;
        }

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
                if(!string.IsNullOrWhiteSpace(joinCode))
                {
                    newGroup.JoinCode = joinCode;
                }
                else
                {
                    MessageBox.Show("Join Code cannot be empty");
                }
                
            }


            groupsList.Add(newGroup);

            SaveGroupListToFile();

            //  Button groupButton = GroupButton_UI(newGroupName);
            Button groupButton = GroupButton_UI(newGroup);
            // Access the ChatContainer from Window1 and add the button to it
            if (mainWindow != null)
            {
                StackPanel chatContainer = mainWindow.ChatContainer;
                chatContainer.Children.Add(groupButton);
            }

            // Add the click event handler for the newly created button
            groupButton.Click += (s, args) =>
            {
                Button clickedButton = (Button)s;
                string groupName = clickedButton.Content.ToString();

                // Create an instance of the ChatHistoryPage and navigate to it
                Page1 chatHistoryPage = new Page1();
                chatHistoryPage.SetChatHistory(groupName); // Pass the group name to set chat history
                mainWindow.ChatBox.NavigationService.Navigate(chatHistoryPage);
            };
        }

        private void GroupButton_Click(object sender, RoutedEventArgs e)
        {
             Button clickedButton = (Button)sender;
         string groupName = clickedButton.Content.ToString();

        }
        private void SaveGroupListToFile()
        {
            try
            {
                // Specify the file path where you want to save the group list
                string filePath = "groupList.txt";

                // Create a list of strings to represent each ChatGroup
                List<string> groupLines = new List<string>();

                foreach (var group in groupsList)
                {
                    if (group.IsPrivate)
                    {
                        // For private groups, save the join code along with the name and private flag
                        groupLines.Add($"{group.Name},{group.IsPrivate},{group.JoinCode}");
                    }
                    else
                    {
                        // For public groups, save only the name and private flag
                        groupLines.Add($"{group.Name},{group.IsPrivate}");
                    }
                }

                // Write the list of strings to the text file
                System.IO.File.WriteAllLines(filePath, groupLines);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during file saving
                // You can log the error or take appropriate action
            }
        }

        private void LoadGroupListFromFile()
        {
            try
            {
                // Specify the file path from where you want to load the group list
                string filePath = "groupList.txt";

                // Check if the file exists
                if (System.IO.File.Exists(filePath))
                {
                    // Read all lines from the text file into a string array
                    string[] lines = System.IO.File.ReadAllLines(filePath);

                    // Clear the existing groups list
                    groupsList.Clear();

                    // Iterate through each line and split it into fields
                    foreach (string line in lines)
                    {
                        string[] fields = line.Split(',');

                        // Ensure that there is at least one field (Name)
                        if (fields.Length >= 1)
                        {
                            // Parse the fields and create a ChatGroup object
                            string groupName = fields[0];
                            bool isPrivate = false; // Default to public group

                            if (fields.Length >= 2)
                            {
                                isPrivate = Convert.ToBoolean(fields[1]);
                            }

                            ChatGroup chatGroup = new ChatGroup
                            {
                                Name = groupName,
                                IsPrivate = isPrivate,
                            };

                            if (isPrivate && fields.Length >= 3)
                            {
                                // If it's a private group and there is a third field (JoinCode)
                                chatGroup.JoinCode = fields[2];
                            }

                            // Add the ChatGroup object to the groupsList
                            groupsList.Add(chatGroup);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during file loading
                // You can log the error or take appropriate action
            }
        }



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
            //Console.WriteLine(baseDirectory);
            string imageRelativePath = "groupsLogo.png";
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
                // Add other customizations as needed
            };

            groupButton.Click += GroupButton_Click; // Handle button click event
            return groupButton;
        }

    }
}
