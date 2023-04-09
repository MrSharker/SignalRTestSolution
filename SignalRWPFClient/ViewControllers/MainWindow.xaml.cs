using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Windows;

namespace SignalRWPFClient.ViewControllers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            await Globals.Connection.InvokeAsync("Send", "", $"User {Globals.LoggedInUser.Username} exits the chat");
            await Globals.Connection.StopAsync();   // отключение от хаба
        }
    }
}
