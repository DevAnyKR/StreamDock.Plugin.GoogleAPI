using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Google.Apis.Adsense.v2;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace StreamDock.Plugin.GoogleAPIs.AdSenseManagement
{
    internal class Payments
    {
        private static readonly int MaxListPageSize = 50;

        private async Task Run()
        {
            GoogleWebAuthorizationBroker.Folder = "StreamDock";

            UserCredential credential;
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { AdsenseService.Scope.AdsenseReadonly },
                    "user", CancellationToken.None);
            }

            // 서비스 생성
            var service = new AdsenseService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "StreamDock"
            });

            // Execute Publisher calls
            ManagementApiConsumer managementApiConsumer = new ManagementApiConsumer(service, MaxListPageSize);
            managementApiConsumer.RunCallPayment();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
