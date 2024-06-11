using System.Drawing;

using BarRaider.SdTools;

using Google.Apis.Auth.OAuth2;

namespace StreamDock.Plugin.GoogleAPIs.AdSenseManagement
{
    /// <summary>
    /// 애드센스를 관리하는 최상위 클래스
    /// </summary>
    internal abstract class AdSense
    {
        protected Color bgColor;

        protected readonly ISDConnection sDConnection;

        internal AdSense(ISDConnection sDConnection, ViewType viewType)
        {
            this.sDConnection = sDConnection;

            GoogleWebAuthorizationBroker.Folder = "StreamDock Plugin";

        }
        internal virtual void OnTick()
        {
        }

        internal virtual void OnPress()
        {
        }
        protected void SetImageKey(string value)
        {
            bgColor = Color.Black;
            var keyImage = ImageHelper.GetImage(bgColor);
            keyImage = ImageHelper.SetImageText(keyImage, value);
            sDConnection.SetImageAsync(keyImage);
        }
    }
}
