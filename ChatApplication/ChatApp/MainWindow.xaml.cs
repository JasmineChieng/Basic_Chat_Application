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
using System.ServiceModel;
using BusinessTierServer;
using System.IO;
using System.Drawing;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Net.NetworkInformation;
using System.Reflection;

namespace ChatApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    

    public partial class MainWindow : Window
    {
        Window3 registration;
        private BusinessInterface foob;

        public MainWindow()
        {
            InitializeComponent();
            ChannelFactory<BusinessInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            string URL = "net.tcp://localhost:8200/DataBusinessService";
            foobFactory = new ChannelFactory<BusinessInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
            registration = new Window3();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {

            string username = usernameTB.Text;
            string password = passwordTB.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) )
            {
                MessageBox.Show("Please fill up all field");
                //Console.WriteLine("username is empty");

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
