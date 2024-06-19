using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Google.Apis.Adsense.v2;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;

namespace StreamDock.Plugins.GoogleAPIs
{
    /// <summary>
    /// API 인증 정보 클래스
    /// </summary>
    internal sealed class GoogleAuth
    {
        internal static readonly string TokenPath = @"DevAny\StreamDock.Plugins\GoogleAPI";
        internal static string TokenUser = "user";
        internal static readonly string TokenFile = $"Google.Apis.Auth.OAuth2.Responses.TokenResponse-{TokenUser}";
        private static UserCredential userCredential;
        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        /// <summary>
        /// 사용자 인증 정보를 가져옵니다.
        /// </summary>
        /// <returns></returns>
        private static async Task<UserCredential> GetCredential()
        {

            if (userCredential != null && !userCredential.Token.IsStale)
            {
                return userCredential;
            }

            var scopes = new string[]
            {
                Oauth2Service.Scope.UserinfoEmail,
                AdsenseService.Scope.AdsenseReadonly,
                CalendarService.Scope.Calendar,
            };

            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                GoogleWebAuthorizationBroker.Folder = TokenPath;
                userCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.FromStream(stream).Secrets, scopes, TokenUser, CancellationToken.None);

                if (userCredential.Token.IsStale)
                    await userCredential.RefreshTokenAsync(CancellationToken.None);
            }

            return userCredential;
        }
        /// <summary>
        /// 인증 서비스를 초기화합니다.
        /// </summary>
        /// <returns></returns>
        internal static async Task<BaseClientService.Initializer> GetServiceInitializer()
        {
            BaseClientService.Initializer init = null;

            if (await semaphoreSlim.WaitAsync(1 * 1000))
            {
                try
                {
                    init = new BaseClientService.Initializer
                    {
                        HttpClientInitializer = await GetCredential(),
                        ApplicationName = "StreamDock Plugin",
                    };
                }
                finally
                {
                    semaphoreSlim.Release();
                }
            }
            else
            {
                semaphoreSlim.Release();
            }
            return init;
        }
        internal static bool CredentialIsExist()
        {
            //TODO 구글 메서드로 유효성 점검
            //! UserCredential 인증 과정 보류 시 프로세스가 멈추므로 토큰 파일 직접 확인
            return File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), TokenPath, TokenFile));
        }
    }
}
