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
            var message = new Message()
            {
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                },
                Token = token
            };

            var reponse = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            Console.WriteLine($"Successfully: {reponse}");
            return reponse;

        }
    }
}
