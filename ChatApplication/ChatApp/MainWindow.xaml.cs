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
            registration = new Window3(foob);

        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {

            string username = usernameTB.Text;
            string password = passwordTB.Text;


            User user = foob.LoginUser(username);

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) )
            {
                MessageBox.Show("Please fill up all field");

            }
            else
            {
                if(user!=null)
                {
                    if(user.Password.Equals(password))
                    {
                        Window1 chatWindow = new Window1(foob,user);
                        this.Visibility = Visibility.Hidden;
                        chatWindow.Show();
                    }
                    else
                    {
                        MessageBox.Show("Wrong Password. Please try again");
                    }

                }
                else
                {
                    MessageBox.Show("User not found. Kindly register yourself first");
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
