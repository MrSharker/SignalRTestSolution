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
    /// Логика взаимодействия для Messanger.xaml
    /// </summary>
    public partial class Messanger : Page
    {
        public Messanger()
        {
            InitializeComponent();
            RegisterAllHandlers();
        }

        private async void Send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Globals.Connection.InvokeAsync("Send", messageTextBox.Text, Globals.LoggedInUser.Username);
            }
            catch (Exception ex)
            {
                chatbox.Items.Add(ex.Message);
            }
        }

        private async void SendTo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Globals.Connection.InvokeAsync("SendTo", messageToTextBox.Text, ToTextBox.Text);
            }
            catch (Exception ex)
            {
                chatbox.Items.Add(ex.Message);
            }
        }

        private async void SendPresent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Globals.Connection.InvokeAsync("SendPresent", presentTextBox.Text, Globals.LoggedInUser.Username);
            }
            catch (Exception ex)
            {
                chatbox.Items.Add(ex.Message);
            }
        }

        private void RegisterAllHandlers()
        {
            Globals.Connection.On<string>("ReceivePresent", (message) =>
            {
                Dispatcher.Invoke(() =>
                {
                    var newMessage = $"{message}";
                    chatbox.Items.Insert(0, newMessage);
                });
            });

            Globals.Connection.On<string, string>("Receive", (message, user) =>
            {
                Dispatcher.Invoke(() =>
                {
                    var newMessage = $"{user}: {message}";
                    chatbox.Items.Insert(0, newMessage);
                });
            });

            Globals.Connection.On<string, string, string>("ReceiveSecret", (message, user, receiver) =>
            {
                Dispatcher.Invoke(() =>
                {
                    var newMessage = $"{user}(private message with {receiver}): {message}";
                    chatbox.Items.Insert(0, newMessage);
                });
            });

            Globals.Connection.On<string>("Notify", (message) =>
            {
                Dispatcher.Invoke(() =>
                {
                    var newMessage = $"{message}";
                    chatbox.Items.Insert(0, newMessage);
                });
            });
        }
    }
}
