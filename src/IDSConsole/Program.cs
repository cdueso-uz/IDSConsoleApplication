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
                HttpResponseMessage responseAuthenticate = await client.PostAsync("https://localhost:5001/co/Authenticate", request);

                IEnumerable<string> cookies = responseAuthenticate.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;




            }
            catch(Exception ex)
            {
                Console.WriteLine("An exception ocurred! Exception message: ", ex.Message);
            }
        }
    }
}
