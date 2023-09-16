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

namespace ChatApp
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public string EnteredCode { get; private set; }
        public Window2()
        {
            InitializeComponent();
        }

        private void JoinButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the entered code from the text box
            EnteredCode = CodeTextBox.Text;

            // Close the dialog with a "true" result
            DialogResult = true;
        }
        }


    
}
