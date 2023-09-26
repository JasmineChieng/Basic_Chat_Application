using BusinessTierServer;
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
        private BusinessInterface foob;
        public Window3(BusinessInterface foob)
        {
            InitializeComponent();
            this.foob = foob;
        }

        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameTB.Text;
            string password = passwordTB.Text;
            string retype_password = retype_pwTB.Text;

            // Create a User object
            User newUser = new User
            {
                Username = username,
                Password = password
            };
            // Call the service to register the user
            bool registrationResult = foob.RegisterUser(newUser);

            if (string.IsNullOrWhiteSpace(username) ||string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(retype_password))
            {
                MessageBox.Show("Please fill up all field");

            }
            else if(!retype_password.Equals(password))
            {
                MessageBox.Show("Retype password must be equal to the password");
            }
            else if (registrationResult)
            {
                MessageBox.Show("Registration Successful");

                MainWindow login = new MainWindow();
                this.Visibility = Visibility.Hidden;
                login.Show();
            }
            else if(!registrationResult)
            {
                MessageBox.Show("Username already exists. Please try another username ");
               
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
