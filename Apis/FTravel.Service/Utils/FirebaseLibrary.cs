using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Utils
{
    public class FirebaseLibrary
    {

        public static async Task<string> SendMessageFireBase(string title, string body, string token)
        {
            try
            {
                var message = new Message()
                {
                    Notification = new Notification()
                    {
                        Title = title,
                        Body = body,
                        ImageUrl = "https://firebasestorage.googleapis.com/v0/b/swd392-d2c4e.appspot.com/o/FTravel%2FLogo_FTravel_3.png?alt=media&token=744b0241-f414-4139-affa-5c523c3bcbc2"
                    },
                    Token = token
                };

                var reponse = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                return reponse;
            }
            catch
            {
                return "";
            }
        }

        public static async Task<bool> SendRangeMessageFireBase(string title, string body, List<string> tokens)
        {
            var message = new MulticastMessage()
            {
                Notification = new Notification()
                {
                    Title = title,
                    Body = body,
                    ImageUrl = "https://firebasestorage.googleapis.com/v0/b/swd392-d2c4e.appspot.com/o/FTravel%2FLogo_FTravel_3.png?alt=media&token=744b0241-f414-4139-affa-5c523c3bcbc2"
                },
                Tokens = tokens
            };

            var reponse = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
            return true;

        }

        public static async Task<string> SendMessagePaymentFireBase(string title, string body, string token)
        {
            try
            {
                var message = new Message()
                {
                    Notification = new Notification()
                    {
                        Title = title,
                        Body = body,
                        ImageUrl = "https://firebasestorage.googleapis.com/v0/b/swd392-d2c4e.appspot.com/o/FTravel%2FLogo_FTravel_3.png?alt=media&token=744b0241-f414-4139-affa-5c523c3bcbc2"
                    },
                    Token = token
                };

                var reponse = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                return reponse;
            }
            catch
            {
                return "";
            }
        }
    }
}
