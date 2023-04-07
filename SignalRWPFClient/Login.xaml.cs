using Microsoft.AspNetCore.SignalR.Client;
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

namespace SignalRWPFClient
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = userTextBox.Text;
                string password = passwordTextBox.Password;

                ApiOperations ops = new ApiOperations();
                User user = await ops.AuthenticateUser(username, password);
                if (user == null)
                {
                    MessageBox.Show("Invalid username or password");
                    return;
                }

                Globals.LoggedInUser = user;
                Globals.Connect();
                
                MessageBox.Show("Login successful");
                NavigationService.Navigate(new Messanger());
            }
            catch (Exception ex)
            {
                chatbox.Items.Add(ex.Message);
            }
        }
    }
}
