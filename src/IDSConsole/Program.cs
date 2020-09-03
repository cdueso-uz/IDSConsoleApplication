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
        private static string hostName = "192.168.7.250";
        //private static string hostName = "localhost";

        public static async Task Main(string[] args)
        {
            var client = new HttpClient(new HttpClientHandler
            {
                AllowAutoRedirect = true,
                MaxAutomaticRedirections = 20
            });

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
                var responseAuthenticate = await client.PostAsync($"http://{hostName}:5000/co/Authenticate", request);
                var cookies = responseAuthenticate.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;

                //2 - connect/Authorizet
                var authorizeQueryString = $"http://{hostName}:5000/connect/authorize?client_id=client&scope=openid profile email offline_access&" + 
                                           $"response_type=code&redirect_uri=https://www.google.com&state=Tj0xpy1Q" +
                                           $"&connection=UZManagerUsersMigration-dev&realm=UZManagerUsersMigration-dev&login_ticket=R-SvDuFgyPbjKRdBSyGXPN4Nl0m4euZP";
                var responseAuthorize = await client.GetAsync(authorizeQueryString);
                var authorizeCode= responseAuthorize.RequestMessage.RequestUri.Query;

            }
            catch(Exception ex)
            {
                Console.WriteLine("An exception ocurred! Exception message: ", ex.Message);
            }
        }
    }
}
