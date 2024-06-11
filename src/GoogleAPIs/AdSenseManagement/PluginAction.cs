using System;
using System.Drawing;
using System.Threading.Tasks;

using BarRaider.SdTools;
using BarRaider.SdTools.Wrappers;

using Newtonsoft.Json.Linq;

namespace StreamDock.Plugin.GoogleAPIs.AdSenseManagement
{
    // manifest.json 에서 지정한 UUID
    [PluginActionId("kr.devany.googleapi.adsensemanagement")]
    public class PluginAction : KeypadBase
    {
        private AdSense adSense;
        private readonly PluginSettings settings;

        public PluginAction(ISDConnection connection, InitialPayload payload) : base(connection, payload)
        {
            this.settings = ((payload.Settings == null || payload.Settings.Count == 0) ? PluginSettings.CreateDefaultSettings() : payload.Settings.ToObject<PluginSettings>());

            Connection.OnApplicationDidLaunch += Connection_OnApplicationDidLaunch;
            Connection.OnApplicationDidTerminate += Connection_OnApplicationDidTerminate;
            Connection.OnDeviceDidConnect += Connection_OnDeviceDidConnect;
            Connection.OnDeviceDidDisconnect += Connection_OnDeviceDidDisconnect;
            Connection.OnPropertyInspectorDidAppear += Connection_OnPropertyInspectorDidAppear;
            Connection.OnPropertyInspectorDidDisappear += Connection_OnPropertyInspectorDidDisappear;
            Connection.OnSendToPlugin += Connection_OnSendToPlugin;
            Connection.OnTitleParametersDidChange += Connection_OnTitleParametersDidChange;
        }

        /// <summary>
        /// 제목 변경 이벤트 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnTitleParametersDidChange(object sender, BarRaider.SdTools.Wrappers.SDEventReceivedEventArgs<BarRaider.SdTools.Events.TitleParametersDidChange> e)
        {
        }

        /// <summary>
        /// 플러그인으로 전송 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnSendToPlugin(object sender, BarRaider.SdTools.Wrappers.SDEventReceivedEventArgs<BarRaider.SdTools.Events.SendToPlugin> e)
        {
        }

        /// <summary>
        /// 속성 숨김 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnPropertyInspectorDidDisappear(object sender, BarRaider.SdTools.Wrappers.SDEventReceivedEventArgs<BarRaider.SdTools.Events.PropertyInspectorDidDisappear> e)
        {
        }

        /// <summary>
        /// 속성 표시 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnPropertyInspectorDidAppear(object sender, BarRaider.SdTools.Wrappers.SDEventReceivedEventArgs<BarRaider.SdTools.Events.PropertyInspectorDidAppear> e)
        {
        }

        /// <summary>
        /// 장치 연결 끊김 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnDeviceDidDisconnect(object sender, BarRaider.SdTools.Wrappers.SDEventReceivedEventArgs<BarRaider.SdTools.Events.DeviceDidDisconnect> e)
        {
        }

        /// <summary>
        /// 장치 연결 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnDeviceDidConnect(object sender, BarRaider.SdTools.Wrappers.SDEventReceivedEventArgs<BarRaider.SdTools.Events.DeviceDidConnect> e)
        {
            UpdateSettingsEnum();
            GetAdSenseInstance();
        }

        /// <summary>
        /// 앱 종료 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnApplicationDidTerminate(object sender, BarRaider.SdTools.Wrappers.SDEventReceivedEventArgs<BarRaider.SdTools.Events.ApplicationDidTerminate> e)
        {
        }

        /// <summary>
        /// 앱 실행 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnApplicationDidLaunch(object sender, BarRaider.SdTools.Wrappers.SDEventReceivedEventArgs<BarRaider.SdTools.Events.ApplicationDidLaunch> e)
        {
        }

        public override void Dispose()
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "Destructor called");
        }
        public async override void KeyPressed(KeyPayload payload)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "Key Pressed");

            TitleParameters tp = new TitleParameters(new FontFamily("Arial"), FontStyle.Bold, 20, Color.White, true, TitleVerticalAlignment.Middle);
            using (Image image = Tools.GenerateGenericKeyImage(out Graphics graphics))
            {
                graphics.FillRectangle(new SolidBrush(Color.Yellow), 0, 0, image.Width, image.Height);
                graphics.AddTextPath(tp, image.Height, image.Width, "처리 중...", Color.Black, 7);
                graphics.Dispose();

                await Connection.SetImageAsync(image);
            }

            UpdateKey();
        }

        public async override void KeyReleased(KeyPayload payload)
        {
        }

        public override void OnTick()
        {
        }

        public override void ReceivedSettings(ReceivedSettingsPayload payload)
        {
            Tools.AutoPopulateSettings(settings, payload.Settings);

            UpdateSettingsEnum();
            GetAdSenseInstance();

            SaveSettings();
        }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload)
        {
        }

        #region Private Methods
        private Task SaveSettings()
        {
            return Connection.SetSettingsAsync(JObject.FromObject(settings));
        }
        #endregion

        // 사용자 정의 메서드
        public void UpdateKey()
        {
            try
            {
                GetAdSenseInstance().OnPress();
            }
            catch (Exception ex)
            {
                Logger.Instance.LogMessage(TracingLevel.ERROR, ex.Message);
                Logger.Instance.LogMessage(TracingLevel.ERROR, ex.StackTrace);
            }
        }

        private void UpdateSettingsEnum()
        {
            ViewType viewType;
            Enum.TryParse<ViewType>(settings.SelectViewType, true, out viewType);
            settings.ViewType = viewType;
        }
        private AdSense GetAdSenseInstance()
        {
            adSense = AdSenseFactory.GetAdSenseInstance(Connection, settings);
            return adSense;
        }
    }
}
