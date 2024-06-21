using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using BarRaider.SdTools;

using Google.Apis.Adsense.v2;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Gmail.v1;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;

namespace StreamDock.Plugins.GoogleAPIs
{
    /// <summary>
    /// API 인증 정보 클래스
    /// </summary>
    internal class GoogleAuth
    {
        internal static readonly string TokenPath = @"DevAny\StreamDock.Plugins\GoogleAPI";
        internal static readonly string TokenFilePrefix = $"Google.Apis.Auth.OAuth2.Responses.TokenResponse-";
        internal UserCredential userCredential;
        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);

        internal GoogleAuth()
        {

        }
        /// <summary>
        /// 사용자 인증 정보를 가져옵니다.
        /// </summary>
        /// <returns></returns>
        private async Task<UserCredential> GetCredentialAsync(string user)
        {
            if (userCredential != null && !userCredential.Token.IsStale)
            {
                return userCredential;
            }

            // API 서비스를 추가합니다.
            var scopes = new string[]
            {
                Oauth2Service.Scope.UserinfoEmail,
                AdsenseService.Scope.AdsenseReadonly,
                CalendarService.Scope.CalendarReadonly,
                GmailService.Scope.GmailReadonly
            };

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(60 * 1000);
            CancellationToken token = cancellationTokenSource.Token;

            try
            {
                using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
                {
                    GoogleWebAuthorizationBroker.Folder = TokenPath;
                    userCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.FromStream(stream).Secrets, scopes, user, token);

                    if (userCredential.Token.IsStale)
                        await userCredential.RefreshTokenAsync(CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                userCredential = null;
                Logger.Instance.LogMessage(TracingLevel.ERROR, ex.Message);
                Logger.Instance.LogMessage(TracingLevel.ERROR, ex.StackTrace);
            }

            return userCredential;
        }
        /// <summary>
        /// 인증 서비스를 초기화합니다.
        /// </summary>
        /// <returns></returns>
        internal async Task<BaseClientService.Initializer> GetServiceInitializerAsync(string user)
        {
            BaseClientService.Initializer init = null;

            if (await semaphoreSlim.WaitAsync(2 * 1000))
            {
                try
                {
                    init = new BaseClientService.Initializer
                    {
                        HttpClientInitializer = await GetCredentialAsync(user),
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
        internal static bool CredentialExist(string user)
        {
            //TODO 구글 메서드로 유효성 점검
            //! UserCredential 인증 과정 보류 시 프로세스가 멈추므로 토큰 파일 직접 확인
            return File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), TokenPath, $"{TokenFilePrefix}{user}"));
        }
    }
}
