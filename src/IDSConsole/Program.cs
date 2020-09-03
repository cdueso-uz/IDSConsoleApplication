using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IDSConsole.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;

namespace IDSConsole
{
    public class Program
    {
        //private static string hostAddress = "192.168.7.250:5001";
        private static string hostAddress = "https://localhost:5001";

        public static async Task Main(string[] args)
        {
            var client = new HttpClient();

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
                //1 - Send Authenticate call
                var responseAuthenticate = await client.PostAsync($"{hostAddress}/co/Authenticate", request);
                var cookies = responseAuthenticate.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;

                //2 - connect/Authorize
                var authorizeQueryString = $"{hostAddress}/connect/authorize?client_id=client&scope=openid profile email offline_access&" +
                                           $"response_type=code&redirect_uri=https://www.google.com&state=Tj0xpy1Q" +
                                           $"&connection=UZManagerUsersMigration-dev&realm=UZManagerUsersMigration-dev&login_ticket=R-SvDuFgyPbjKRdBSyGXPN4Nl0m4euZP";
                var responseAuthorize = await client.GetAsync(authorizeQueryString);
                var codeAuthorize = QueryHelpers.ParseQuery(responseAuthorize.RequestMessage.RequestUri.Query)["code"].First();

                //3 - connect/Token
                var tokenClient = new HttpClient();

                var keyValuePairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("client_id", "client"),
                    new KeyValuePair<string, string>("client_secret", "secret"),
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("code", codeAuthorize),
                    new KeyValuePair<string, string>("redirect_uri", "https://www.google.com")
                };

                var tokenRequest = new HttpRequestMessage(HttpMethod.Post, $"{hostAddress}/connect/token")
                {
                    Content = new FormUrlEncodedContent(keyValuePairs)
                };

                var responseToken = await tokenClient.SendAsync(tokenRequest);

                Console.WriteLine(responseToken.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception ocurred! Exception message: ", ex.Message);
            }
        }
    }
}