using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Windows;

namespace SignalRWPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HubConnection connection;
        public MainWindow()
        {
            InitializeComponent();
            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7219/chat")
                .ConfigureLogging(logging =>
                {
                    logging.SetMinimumLevel(LogLevel.Debug);
                })
                .WithAutomaticReconnect()
                .Build();

            connection.On<string>("ReceivePresent", (message) =>
            {
                Dispatcher.Invoke(() =>
                {
                    var newMessage = $"{message}";
                    chatbox.Items.Insert(0, newMessage);
                });
            });

            connection.On<string, string>("Receive", (message, user) =>
            {
                Dispatcher.Invoke(() =>
                {
                    var newMessage = $"{user}: {message}";
                    chatbox.Items.Insert(0, newMessage);
                });
            });

            connection.On<string>("Notify", (message) =>
            {
                Dispatcher.Invoke(() =>
                {
                    var newMessage = $"{message}";
                    chatbox.Items.Insert(0, newMessage);
                });
            });
        }
        
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await connection.StartAsync();
                chatbox.Items.Add("You are entered into the chat");
                sendBtn.IsEnabled = true;
                sendPresemtBtn.IsEnabled = true;
            }
            catch (Exception ex)
            {
                chatbox.Items.Add(ex.Message);
            }
        }
        
        private async void Send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await connection.InvokeAsync("Send", messageTextBox.Text, userTextBox.Text);
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
                await connection.InvokeAsync("SendPresent", presentTextBox.Text, userTextBox.Text);
            }
            catch (Exception ex)
            {
                chatbox.Items.Add(ex.Message);
            }
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            await connection.InvokeAsync("Send", "", $"User {userTextBox.Text} exits the chat");
            await connection.StopAsync();   // отключение от хаба
        }
    }
}
