using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using UserDLL;

namespace ChatApp
{
    /// <summary>
    /// Interaction logic for Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        private List<User> userList = new List<User>();
        public Window3()
        {
            InitializeComponent();
            LoadUserData();
        }

        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameTB.Text;
            string password = passwordTB.Text;
            string retype_password = retype_pwTB.Text;

            if(string.IsNullOrWhiteSpace(username) ||string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(retype_password))
            {
                MessageBox.Show("Please fill up all field");

            }
            else if(!retype_password.Equals(password))
            {
                MessageBox.Show("Retype password must be equal to the password");
            }
            else if(UserExists(username))
            {
                MessageBox.Show("Username already exists. Please try another username ");
            }
            else
            {
                MessageBox.Show("Registration Successful");

                User user = new User();
                user.Username = username;
                user.Password = password;
                
                userList.Add(user);
                SaveUserData();

                MainWindow login = new MainWindow();
                this.Visibility = Visibility.Hidden;
                login.Show();

                

            }
        }

        // Function to check if a username already exists in the userList
        public bool UserExists(string username)
        {
            return userList.Any(user => user.Username == username);
        }
   // Function to get the userList
        public List<User> GetUserList()
        {
            return userList;
        }

        public void SaveUserData()
        {
            try
            {
                // Specify the file path where you want to save user data as text
                string filePath = "userList.txt";

                // Create a StreamWriter to write data to the file
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (User user in userList)
                    {
                        // Write user data as plain text in the format "Username,Password"
                        writer.WriteLine($"{user.Username},{user.Password}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during file saving
                // You can log the error or take appropriate action
            }
        }

        public void LoadUserData()
        {
            try
            {
                // Specify the file path from where you want to load user data as text
                string filePath = "userList.txt";

                // Check if the file exists
                if (File.Exists(filePath))
                {
                    // Create a StreamReader to read data from the file
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            // Split the line into username and password
                            string[] parts = line.Split(',');
                            if (parts.Length == 2)
                            {
                                User user = new User
                                {
                                    Username = parts[0],
                                    Password = parts[1]
                                };
                                userList.Add(user);
                            }
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

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow login = new MainWindow();
            this.Visibility = Visibility.Hidden;
            login.Show();
        }
    }
}
