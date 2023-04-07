using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SignalRWPFClient
{
    class ApiOperations
    {
        private string baseUrl;

        public ApiOperations()
        {
            this.baseUrl = "https://localhost:7219";
        }

        public async Task<User> AuthenticateUser(string username, string password)
        {
            string json = JsonConvert.SerializeObject(new
            {
                email = username,
                password = password
            });

            HttpClient client = new();
            client.BaseAddress = new Uri(baseUrl);
            try
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/login", content);
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(responseBody);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
