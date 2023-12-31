﻿using BusinessTierServer;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserDLL;

namespace ChatApp
{
    /// <summary>
    /// Interaction logic for Page4.xaml
    /// </summary>
    public partial class Page4 : Page
    {
        private List<ChatGroup> groupsList = new List<ChatGroup>();
        private User user;
        private BusinessInterface foob;
        private Window1 chatWindow;
        private Page3 page3;
        public Page4(Page3 page3, User user, BusinessInterface foob, Window1 chatWindow)
        {
            InitializeComponent();
            this.user = user;
            this.page3 = page3;
            groupsList = page3.GroupList;
            this.foob = foob;
            this.chatWindow = chatWindow;

            // Clear the existing content of the container
            Page4Container.Children.Clear();

            // Iterate through the group list and create a card for each group that was available
            foreach (var group in groupsList)
            {
                CreateGroupCard(group);
            }
        }

        //Dynamic group card UI 
        private void CreateGroupCard(ChatGroup group)
        {
            // Create a card to display the group
            Border cardBorder = new Border
            {
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Orange,
                CornerRadius = new CornerRadius(5),
                Margin = new Thickness(10),
                Padding = new Thickness(10),
                Width = 500
            };
            // Create TextBlock to display group name
            TextBlock groupNameTextBlock = new TextBlock
            {
                Text = group.Name,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
            };

            // Create a Button to join the group
            Button joinButton = new Button
            {
                Content = "Join",
                Width = 60,
                Height = 30,
                BorderBrush = Brushes.Orange,
                Background = Brushes.Orange,
            };

            // Create a Grid to arrange the elements
            Grid grid = new Grid();

            // Add columns to the Grid
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) }); // Auto-sized column for the group name
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(350) }); // Auto-sized column for the private label
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(60) }); // Auto-sized column for the join button

            //If is private group add a private icon
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string imageRelativePath = "privateIcon.png";
            string imagePath = System.IO.Path.Combine(baseDirectory, imageRelativePath);

            // Create the Label for private groups
            Label privateLabel = new Label
            {
                Content = "Private",
                Background = new ImageBrush(new BitmapImage(new Uri(imagePath))),
                Width = 20,
                Height = 20,
                Margin = new Thickness(2),

            };

            // Place the elements in the Grid
            Grid.SetColumn(groupNameTextBlock, 1); // Set the group name TextBlock to the first column
            Grid.SetColumn(privateLabel, 0);       // Set the private label to the second column
            Grid.SetColumn(joinButton, 2);         // Set the join button to the third column

            //If is private , need show the enter code dialog
            grid.Children.Add(groupNameTextBlock);
            grid.Children.Add(privateLabel);
            grid.Children.Add(joinButton);
            if (group.IsPrivate)
            {
                privateLabel.Visibility = Visibility.Visible;
                joinButton.Click += (s, args) =>
                {
                    OpenJoinCodeDialog(group);

                };
            }

            else
            {
                //If not private, then hide the private logo
                privateLabel.Visibility = Visibility.Collapsed;
                // Handle the join button click event
                joinButton.Click += (s, args) =>
                {

                    if (foob.JoinGroupChat(group, user))
                    {
                        MessageBox.Show($"Joined group: {group.Name}");

                        Button groupButton = page3.GroupButton_UI(group);
                        if (chatWindow != null)
                        {
                            StackPanel chatContainer = chatWindow.ChatContainer;
                            chatContainer.Children.Add(groupButton);
                        }

                        groupButton.Click += (ss, argss) =>
                        {
                            Button clickedButton = (Button)s;
                            string groupName = clickedButton.Content.ToString();

                            Page1 chatHistoryPage = new Page1(chatWindow, user, group, foob);
                            chatWindow.ChatBox.NavigationService.Navigate(chatHistoryPage);
                        };

                    }
                    else
                    {
                        MessageBox.Show("You are already a member of this group chat.");
                    }

                };
            }



            // Add the Grid to the card container
            cardBorder.Child = grid;

            // Add the card to Page 4
            Page4Container.Children.Add(cardBorder);



        }

        //Calling the enter join code window
        private void OpenJoinCodeDialog(ChatGroup group)
        {
            Window2 dialog = new Window2();

            if (dialog.ShowDialog() == true)
            {
                // Get the entered code from the dialog
                string enteredCode = dialog.EnteredCode;

                // Check if the entered code matches the group's join code
                if (enteredCode == group.JoinCode)
                {
                    // Implement join group functionality here

                    if (foob.JoinGroupChat(group, user))
                    {
                        MessageBox.Show($"Joined group: {group.Name}");
                        Button groupButton = page3.GroupButton_UI(group);
                        if (chatWindow != null)
                        {
                            StackPanel chatContainer = chatWindow.ChatContainer;
                            chatContainer.Children.Add(groupButton);
                        }

                        groupButton.Click += (s, args) =>
                        {
                            Button clickedButton = (Button)s;
                            string groupName = clickedButton.Content.ToString();

                            Page1 chatHistoryPage = new Page1(chatWindow, user, group, foob);
                            chatWindow.ChatBox.NavigationService.Navigate(chatHistoryPage);
                        };

                    }
                    else
                    {
                        MessageBox.Show("You are already a member of this group chat.");
                    }


                }
                else
                {
                    // Display an error message for incorrect code
                    MessageBox.Show("Wrong code. Please try again.");
                }
            }
        }



    }
}
