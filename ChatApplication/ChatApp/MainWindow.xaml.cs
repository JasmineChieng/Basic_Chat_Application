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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Window3 registration;
        public MainWindow()
        {
            InitializeComponent();
            registration = new Window3();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {

            string username = usernameTB.Text;
            string password = passwordTB.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) )
            {
                MessageBox.Show("Please fill up all field");

            }
            else
            {
                List<User> users = registration.GetUserList();
                User userFound=  users.FirstOrDefault(u => u.Username == username);

                if(userFound ==null)
                {
                    MessageBox.Show("User not found. Kindly register yourself first");
                }
                else
                {
                    if (userFound.Password.Equals(password))
                    {
                        Window1 chatWindow = new Window1();
                        this.Visibility = Visibility.Hidden;
                        chatWindow.Show();
                    }
                    else
                    {
                        MessageBox.Show("Wrong Password. Please try again");
                    }
                }


            }
  
        }

        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            registration.Show();
        }
    }


}
