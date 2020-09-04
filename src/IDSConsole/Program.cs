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
        private static string hostAddress = "https://localhost:5001";
        //private static string hostAddress = "https://localhost:5001";

        public static async Task Main(string[] args)
        {
            var clientUICookie = new HttpClient();

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
                var responseAuthenticate = await clientUICookie.PostAsync($"{hostAddress}/co/authenticate", request);
                var cookies = responseAuthenticate.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;

                //2 - connect/Authorize
                var authorizeQueryString = $"{hostAddress}/authorize?client_id=client&scope=openid profile email offline_access&" +
                                           $"response_type=code&redirect_uri=https://www.google.com&state=Tj0xpy1Q" +
                                           $"&connection=UZManagerUsersMigration-dev&realm=UZManagerUsersMigration-dev&login_ticket=R-SvDuFgyPbjKRdBSyGXPN4Nl0m4euZP";
                var responseAuthorize = await clientUICookie.GetAsync(authorizeQueryString);
                var codeAuthorize = QueryHelpers.ParseQuery(responseAuthorize.RequestMessage.RequestUri.Query)["code"].First();

                //3 - connect/Token
                var clientBackend = new HttpClient();

                var keyValuePairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("client_id", "client"),
                    new KeyValuePair<string, string>("client_secret", "secret"),
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("code", codeAuthorize),
                    new KeyValuePair<string, string>("redirect_uri", "https://www.google.com")
                };

                var tokenRequest = new HttpRequestMessage(HttpMethod.Post, $"{hostAddress}/oauth/token")
                {
                    Content = new FormUrlEncodedContent(keyValuePairs)
                };

                var responseToken = await clientBackend.SendAsync(tokenRequest);
                Console.WriteLine(responseToken.Content.ReadAsStringAsync().Result);

                //4 - Logut
                var logoutQueryString = $"{hostAddress}/v2/logout?client_id=client&returnTo=https://www.google.com&auth0Client=patata";
                var responseLogout = await clientUICookie.GetAsync(logoutQueryString);
                var cookiesLogout = responseAuthenticate.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;

            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception ocurred! Exception message: ", ex.Message);
            }
        }
    }
}
