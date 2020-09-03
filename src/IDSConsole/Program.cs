using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IDSConsole.Models;
using Newtonsoft.Json;

namespace IDSConsole
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var client = new HttpClient();

            //1 - Send Authenticate call
            var login = new LoginRequest()
            {
                Client_Id = "patata",
                Credential_Type = "unknow",
                Username = "alice",
                Password = "alice",
                Realm = "db"
            };
            var request = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
            try
            {
                var responseAuthenticate = await client.PostAsync("http://localhost:10001/co/Authenticate", request );
            }
            catch(Exception ex)
            {
                int i = 0;
            }
        }
    }
}
