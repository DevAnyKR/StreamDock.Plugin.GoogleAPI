using System;
using System.Drawing;
using System.Threading.Tasks;

using BarRaider.SdTools;
using BarRaider.SdTools.Wrappers;

using Newtonsoft.Json.Linq;

namespace StreamDock.Plugin.GoogleAPIs.AdSenseManagement
{
    /// <summary>
    /// manifest.json 에서 선언한 플러그인 UUID
    /// </summary>
    [PluginActionId("kr.devany.googleapi.adsensemanagement")]
    public class PluginAction : KeypadBase
    {
        internal KeyImage keyImage;
        internal PluginSettings pluginSettings { get; set; }

        public PluginAction(ISDConnection connection, InitialPayload payload) : base(connection, payload)
        {
            pluginSettings = ((payload.Settings == null || payload.Settings.Count == 0) ? PluginSettings.CreateDefaultSettings() : payload.Settings.ToObject<PluginSettings>());
            keyImage = new();

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
        /// 제목 변경 이벤트 처리. 스트림독에 플러그인이 나타날 때 호출됩니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnTitleParametersDidChange(object sender, BarRaider.SdTools.Wrappers.SDEventReceivedEventArgs<BarRaider.SdTools.Events.TitleParametersDidChange> e)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "OnTitleParametersDidChange Event Handled");

            DisplayInitial();
        }

        /// <summary>
        /// 플러그인으로 전송 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnSendToPlugin(object sender, BarRaider.SdTools.Wrappers.SDEventReceivedEventArgs<BarRaider.SdTools.Events.SendToPlugin> e)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "OnSendToPlugin Event Handled");
        }

        /// <summary>
        /// PropertyInspector가 숨겨질 때마다 호출됩니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnPropertyInspectorDidDisappear(object sender, BarRaider.SdTools.Wrappers.SDEventReceivedEventArgs<BarRaider.SdTools.Events.PropertyInspectorDidDisappear> e)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "OnPropertyInspectorDidDisappear Event Handled");
        }

        /// <summary>
        /// PropertyInspector가 표시될 때마다 호출됩니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnPropertyInspectorDidAppear(object sender, BarRaider.SdTools.Wrappers.SDEventReceivedEventArgs<BarRaider.SdTools.Events.PropertyInspectorDidAppear> e)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "OnPropertyInspectorDidAppear Event Handled");
        }

        /// <summary>
        /// 장치 연결 끊김 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnDeviceDidDisconnect(object sender, BarRaider.SdTools.Wrappers.SDEventReceivedEventArgs<BarRaider.SdTools.Events.DeviceDidDisconnect> e)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "OnDeviceDidDisconnect Event Handled");
        }

        /// <summary>
        /// 장치 연결 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnDeviceDidConnect(object sender, BarRaider.SdTools.Wrappers.SDEventReceivedEventArgs<BarRaider.SdTools.Events.DeviceDidConnect> e)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "OnDeviceDidConnect Event Handled");
        }

        /// <summary>
        /// 앱 종료 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnApplicationDidTerminate(object sender, BarRaider.SdTools.Wrappers.SDEventReceivedEventArgs<BarRaider.SdTools.Events.ApplicationDidTerminate> e)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "OnApplicationDidTerminate Event Handled");
        }

        /// <summary>
        /// 앱 실행 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnApplicationDidLaunch(object sender, BarRaider.SdTools.Wrappers.SDEventReceivedEventArgs<BarRaider.SdTools.Events.ApplicationDidLaunch> e)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "OnApplicationDidLaunch Event Handled");
        }

        /// <summary>
        /// 스트림독에 플러그인이 표시되지 않으면 호출됩니다.
        /// </summary>
        public override void Dispose()
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "Destructor called");
        }

        /// <summary>
        /// 키를 눌렀을 때 호출됩니다.
        /// </summary>
        /// <param name="payload"></param>
        public override void KeyPressed(KeyPayload payload)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "KeyPressed called");
        }

        /// <summary>
        /// 키를 뗄 때 호출됩니다.
        /// </summary>
        /// <param name="payload"></param>
        public async override void KeyReleased(KeyPayload payload)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "KeyReleased called");

            DisplayBusy();
            UpdateApiData();
        }

        /// <summary>
        /// 매 초 호출되는 메서드입니다.
        /// </summary>
        public override void OnTick()
        {
        }

        /// <summary>
        /// PropertyInspector에서 값이 변경될 때마다 호출됩니다.
        /// </summary>
        /// <param name="payload"></param>
        public override void ReceivedSettings(ReceivedSettingsPayload payload)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "ReceivedSettings called");

            Tools.AutoPopulateSettings(pluginSettings, payload.Settings);

            UpdateSettingsEnum();
            SaveSettings();

            UpdateKeyImage();
        }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "ReceivedGlobalSettings called");
        }

        #region Private Methods
        /// <summary>
        /// 설정 값을 스트림독으로 전달합니다.
        /// </summary>
        /// <returns></returns>
        private Task SaveSettings()
        {
            return Connection.SetSettingsAsync(JObject.FromObject(pluginSettings));
        }
        #endregion

        /// <summary>
        /// 초기 이미지를 표시합니다.
        /// </summary>
        public void DisplayInitial()
        {
            if (Item.Instance.DataReceived)
            {
                UpdateKeyImage();
            }
        }

        /// <summary>
        /// 작업 중임을 알리는 이미지를 표시합니다.
        /// </summary>
        public async void DisplayBusy()
        {
            TitleParameters tp = new TitleParameters(new FontFamily("Arial"), FontStyle.Bold, 20, Color.White, true, TitleVerticalAlignment.Middle);
            using (Image image = Tools.GenerateGenericKeyImage(out Graphics graphics))
            {
                graphics.FillRectangle(new SolidBrush(Color.Yellow), 0, 0, image.Width, image.Height);
                graphics.AddTextPath(tp, image.Height, image.Width, "처리 중...", Color.Black, 7);
                graphics.Dispose();

                await Connection.SetImageAsync(image);
            }
        }

        /// <summary>
        /// 데이터를 갱신합니다.
        /// </summary>
        public async void UpdateApiData()
        {
            try
            {
                Item.Instance = await GetApiInstance().Execute();
                UpdateKeyImage();
            }
            catch (Exception ex)
            {
                Logger.Instance.LogMessage(TracingLevel.ERROR, ex.Message);
                Logger.Instance.LogMessage(TracingLevel.ERROR, ex.StackTrace);
            }
        }

        /// <summary>
        /// 키 이미지를 변경합니다.
        /// </summary>
        public void UpdateKeyImage()
        {
            if (!Item.Instance.DataReceived)
            {
                UpdateSettingsEnum();
            }

            var image = ImageHelper.GetImage(pluginSettings.BackColor);

            image = ImageHelper.SetImageText(image, Item.Instance.Value1, new SolidBrush(pluginSettings.FrontColor), 72, Item.Instance.Value2 == null ? 72 : 96);
            if (Item.Instance.Value2 != null)
            {
                image = ImageHelper.SetImageText(image, Item.Instance.Value2, new SolidBrush(pluginSettings.FrontColor), 72, 48);
            }
            keyImage.Final = image;
            Connection.SetImageAsync(keyImage.Final);
        }

        /// <summary>
        /// 스트림독에서 설정한 값을 클래스에 저장합니다.
        /// </summary>
        private void UpdateSettingsEnum()
        {
            try
            {
                ViewTypes viewType;
                Enum.TryParse<ViewTypes>(pluginSettings.PiViewType, true, out viewType);
                pluginSettings.ViewType = viewType;

                pluginSettings.FrontColor = ColorTranslator.FromHtml(pluginSettings.PiFrontColor);
                pluginSettings.BackColor = ColorTranslator.FromHtml(pluginSettings.PiBackColor);

                Item.Instance.Init();
                Item.Instance.Value1 = "Press Key...";
            }
            catch (Exception ex)
            {
                Logger.Instance.LogMessage(TracingLevel.ERROR, ex.Message);
                Logger.Instance.LogMessage(TracingLevel.ERROR, ex.StackTrace);
            }
        }

        /// <summary>
        /// Google API 쿼리 클래스의 인스턴스를 가져옵니다.
        /// </summary>
        /// <returns></returns>
        private ActionProc GetApiInstance()
        {
            return new ActionProc(Connection, pluginSettings.ViewType);
        }
    }
}
