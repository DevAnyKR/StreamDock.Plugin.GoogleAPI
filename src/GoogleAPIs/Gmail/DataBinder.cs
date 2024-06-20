using System.Threading.Tasks;

using Google.Apis.Gmail.v1;
using Google.Apis.Oauth2.v2;

namespace StreamDock.Plugins.GoogleAPIs.Gmail
{
    /// <summary>
    /// API 동작 정의 프로시저
    /// </summary>
    internal class DataBinder
    {
        PluginSettings pluginSettings;
        Item item;
        GoogleAuth googleAuth;
        internal DataBinder(PluginSettings pluginsettings, Item item)
        {
            this.pluginSettings = pluginsettings;
            this.item = item;
            this.googleAuth = new();
        }
        /// <summary>
        /// 키가 눌렸을 때 동작 정의. Google API 통신.
        /// </summary>
        internal async Task<Item> ExecuteAsync()
        {
            // 구글 계정
            Oauth2Service oauth2Service = new Oauth2Service(googleAuth.GetServiceInitializerAsync(pluginSettings.UserTokenName).Result);
            item.UserId = oauth2Service.Userinfo.Get().Execute().Email;

            // 구글 API 통신 인스턴스
            ApiService apiSevice = new ApiService(googleAuth.userCredential, new GmailService(await googleAuth.GetServiceInitializerAsync(pluginSettings.UserTokenName)));

            // StreamDock 설정에 따른 동작 호출
            var request = await apiSevice.GetMessagesListAsync(item.UserId, "UNREAD");

            item.MessageUnReadCount = request.ResultSizeEstimate;

            // 디스플레이용 데이터 가공
            item = SetDisplayValue();

            return item;
        }

        /// <summary>
        /// StreamDock 설정에 따라 표시할 데이터를 지정합니다.
        /// </summary>
        /// <returns></returns>
        internal Item SetDisplayValue()
        {
            item.DisplayValues.Clear();
            item.DisplayValues.Add(item.MessageUnReadCount.ToString());

            return item;
        }
    }
}