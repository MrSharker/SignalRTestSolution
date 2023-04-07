using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SignalRWPFClient
{
    class Globals
    {
        public static User LoggedInUser { get; set; }

        public static HubConnection Connection { get; set; }

        public static async void Connect()
        {
            Connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7219/chat", options => {
                    options.AccessTokenProvider = async () =>
                    {
                        return LoggedInUser?.access_token;
                    };
                })
                .ConfigureLogging(logging =>
                {
                    logging.SetMinimumLevel(LogLevel.Debug);
                })
                .WithAutomaticReconnect()
                .Build();
            await Connection.StartAsync();
        }
    }
}
