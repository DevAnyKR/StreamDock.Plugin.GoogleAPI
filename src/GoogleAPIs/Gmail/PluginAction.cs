using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Threading.Tasks;

using BarRaider.SdTools;
using BarRaider.SdTools.Wrappers;

using Newtonsoft.Json.Linq;

namespace StreamDock.Plugins.GoogleAPIs.Gmail
{
    /// <summary>
    /// manifest.json 에서 선언한 플러그인 UUID
    /// </summary>
    [PluginActionId("kr.devany.googleapi.gmail")]
    public class PluginAction : KeypadBase
    {
        Item item;
        PluginSettings pluginSettings;
        DataBinder dataBinder;

        public PluginAction(ISDConnection connection, InitialPayload payload) : base(connection, payload)
        {
            pluginSettings = (payload.Settings == null || payload.Settings.Count == 0) ? PluginSettings.CreateDefaultSettings() : payload.Settings.ToObject<PluginSettings>();
            item = new();

            Connection.OnApplicationDidLaunch += Connection_OnApplicationDidLaunch;
            Connection.OnApplicationDidTerminate += Connection_OnApplicationDidTerminate;
            Connection.OnDeviceDidConnect += Connection_OnDeviceDidConnect;
            Connection.OnDeviceDidDisconnect += Connection_OnDeviceDidDisconnect;
            Connection.OnPropertyInspectorDidAppear += Connection_OnPropertyInspectorDidAppear;
            Connection.OnPropertyInspectorDidDisappear += Connection_OnPropertyInspectorDidDisappear;
            Connection.OnSendToPlugin += Connection_OnSendToPlugin;
            Connection.OnTitleParametersDidChange += Connection_OnTitleParametersDidChange;
            pluginSettings.PropertyChanged += PropertyChanged;
        }

        /// <summary>
        /// 제목이 변경되거나 스트림독에 플러그인이 나타날 때 호출됩니다.titleParametersDidChange
        /// 하드웨어 기준.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Connection_OnTitleParametersDidChange(object sender, SDEventReceivedEventArgs<BarRaider.SdTools.Events.TitleParametersDidChange> e)
        {
            try
            {
                Logger.Instance.LogMessage(TracingLevel.INFO, "OnTitleParametersDidChange Event Handled");
                if (!GoogleAuth.CredentialIsExist(pluginSettings.UserTokenName))
                {
                    await DisplayInitialAsync();
                }
                else
                {
                    await DisplayBusyAsync();
                    await UpdateApiDataAsync();
                }
            }
            catch (Exception ex)
            {
                await Connection.ShowAlert();
                Logger.Instance.LogMessage(TracingLevel.ERROR, ex.Message);
                Logger.Instance.LogMessage(TracingLevel.ERROR, ex.StackTrace);
            }
        }

        /// <summary>
        /// PropertyInspector가 sendToPlugin 이벤트를 사용할 때 플러그인에서 받은 이벤트입니다.
        /// OnPropertyInspectorDidAppear 이벤트와 동시에 호출됩니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnSendToPlugin(object sender, SDEventReceivedEventArgs<BarRaider.SdTools.Events.SendToPlugin> e)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "OnSendToPlugin Event Handled");
        }

        /// <summary>
        /// PropertyInspector가 표시될 때마다 호출됩니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnPropertyInspectorDidAppear(object sender, SDEventReceivedEventArgs<BarRaider.SdTools.Events.PropertyInspectorDidAppear> e)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "OnPropertyInspectorDidAppear Event Handled");

        }

        /// <summary>
        /// PropertyInspector가 숨겨질 때마다 호출됩니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnPropertyInspectorDidDisappear(object sender, SDEventReceivedEventArgs<BarRaider.SdTools.Events.PropertyInspectorDidDisappear> e)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "OnPropertyInspectorDidDisappear Event Handled");
        }


        /// <summary>
        /// 장치 연결 끊김 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnDeviceDidDisconnect(object sender, SDEventReceivedEventArgs<BarRaider.SdTools.Events.DeviceDidDisconnect> e)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "OnDeviceDidDisconnect Event Handled");
        }

        /// <summary>
        /// 장치 연결 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnDeviceDidConnect(object sender, SDEventReceivedEventArgs<BarRaider.SdTools.Events.DeviceDidConnect> e)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "OnDeviceDidConnect Event Handled");
        }

        /// <summary>
        /// 앱 종료 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnApplicationDidTerminate(object sender, SDEventReceivedEventArgs<BarRaider.SdTools.Events.ApplicationDidTerminate> e)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "OnApplicationDidTerminate Event Handled");
        }

        /// <summary>
        /// 앱 실행 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnApplicationDidLaunch(object sender, SDEventReceivedEventArgs<BarRaider.SdTools.Events.ApplicationDidLaunch> e)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "OnApplicationDidLaunch Event Handled");
        }
        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, $"{e.PropertyName} Property Changed");
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
            try
            {
                Logger.Instance.LogMessage(TracingLevel.INFO, "KeyReleased called");
                if (GoogleAuth.CredentialIsExist(pluginSettings.UserTokenName) && CheckExistData())
                {
                    await Connection.OpenUrlAsync("https://mail.google.com/");
                }
                else
                {
                    await DisplayBusyAsync();
                    await UpdateApiDataAsync();
                }
            }
            catch (Exception ex)
            {
                await Connection.ShowAlert();
                Logger.Instance.LogMessage(TracingLevel.ERROR, ex.Message);
                Logger.Instance.LogMessage(TracingLevel.ERROR, ex.StackTrace);
            }
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
        public async override void ReceivedSettings(ReceivedSettingsPayload payload)
        {
            try
            {
                Logger.Instance.LogMessage(TracingLevel.INFO, "ReceivedSettings called");
                Tools.AutoPopulateSettings(pluginSettings, payload.Settings);
                await SaveSettingsAsync();

                if (!GoogleAuth.CredentialIsExist(pluginSettings.UserTokenName))
                {
                    item.Init();
                    dataBinder = null;
                    await DisplayInitialAsync();
                }
                else if (CheckExistData())
                {
                    await DisplayBusyAsync();
                    await UpdateApiDataAsync();
                }
                else
                {
                    await DisplayInitialAsync();
                }
            }
            catch (Exception ex)
            {
                await Connection.ShowAlert();
                Logger.Instance.LogMessage(TracingLevel.ERROR, ex.Message);
                Logger.Instance.LogMessage(TracingLevel.ERROR, ex.StackTrace);
            }
        }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "ReceivedGlobalSettings called");
            Tools.AutoPopulateSettings(pluginSettings, payload.Settings);
        }

        #region Private Methods
        /// <summary>
        /// 설정 값을 스트림독으로 전달합니다.
        /// </summary>
        /// <returns></returns>
        private async Task SaveSettingsAsync()
        {
            await Connection.SetSettingsAsync(JObject.FromObject(pluginSettings));
        }

        /// <summary>
        /// 기존 데이터가 있는지 검사합니다.
        /// </summary>
        /// <returns></returns>
        private bool CheckExistData()
        {
            return item.MessageUnReadCount > 0;
        }

        /// <summary>
        /// 초기 이미지를 표시합니다.
        /// </summary>
        private async Task DisplayInitialAsync()
        {
            item.DisplayValues.OnlyOne("Press Key...");
            await Connection.SetImageAsync(UpdateKeyImage(item, true)); // 초기 이미지 출력
        }
        private async Task DisplayPreValueAsync()
        {
            try
            {
                UpdateValues();
#if DEBUG
                Logger.Instance.LogMessage(TracingLevel.INFO, "DisplayInitialAsync: 스트림독으로 이미지 전송 중...");
#endif
                await Connection.SetImageAsync(UpdateKeyImage(item, true)); // 초기 이미지 출력
            }
            catch (Exception ex)
            {
                Logger.Instance.LogMessage(TracingLevel.ERROR, ex.Message);
                Logger.Instance.LogMessage(TracingLevel.ERROR, ex.StackTrace);
            }
        }
        /// <summary>
        /// 작업 중임을 알리는 이미지를 표시합니다.
        /// </summary>
        private async Task DisplayBusyAsync()
        {
            TitleParameters tp = new TitleParameters(new FontFamily("Arial"), FontStyle.Bold, 12, Color.White, true, TitleVerticalAlignment.Middle);
            using (Bitmap image = Tools.GenerateGenericKeyImage(out Graphics graphics))
            {
                graphics.FillRectangle(new SolidBrush(Color.Yellow), 0, 0, image.Width, image.Height);
                graphics.AddTextPath(tp, image.Height, image.Width, "Loading...", Color.Black, 7); //TODO 지역화
#if DEBUG
                    Logger.Instance.LogMessage(TracingLevel.INFO, "DisplayBusyAsync: 스트림독으로 이미지 전송 중...");
#endif
                await Connection.SetImageAsync(image);
                graphics.Dispose();
            }
        }
        /// <summary>
        /// PI 설정에 따라 Google API 데이터를 갱신합니다.
        /// </summary>
        private async Task UpdateApiDataAsync()
        {
            item = await GetApiInstance().ExecuteAsync();
            await Connection.SetTitleAsync(UpdateKeyTitle(item));
            Logger.Instance.LogMessage(TracingLevel.INFO, "UpdateApiDataAsync: Sending Image to Stream Dock...");
            await Connection.SetImageAsync(UpdateKeyImage(item));
        }
        /// <summary>
        /// PI 설정에 따라 이미 수신된 Google API 데이터로 갱신합니다.
        /// </summary>
        private void UpdateValues()
        {
            GetApiInstance().SetDisplayValue();
        }
        /// <summary>
        /// 키 이미지를 변경합니다. 출력할 정보를 이미지로 변환합니다.
        /// </summary>
        private Bitmap UpdateKeyImage(Item item, bool autoSize = false)
        {
            // 배경을 만들고
            Bitmap bmp = new Bitmap(ImageHelper.GetImage(pluginSettings.BackColor));

            // 그래픽 작업
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                // 로고를 줄이고
                var bmpGoogleLogo = new Bitmap(Properties.Resources.gmail, new Size(100, 100));

                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                graphics.DrawImage(bmpGoogleLogo, new Point(10, 30));

                // 숫자를 넣고
                var font = new Font("Arial", 48, FontStyle.Bold, GraphicsUnit.Pixel);
                var stringFormat = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                font = autoSize ? ImageHelper.ResizeFont(graphics, item.DisplayValues[0], font) : font;

                // 동그라미
                var fontSize = graphics.MeasureString(item.DisplayValues[0], font);
                float fontMaxSize = fontSize.Width > fontSize.Height ? fontSize.Width : fontSize.Height;
                int padding = 5;
                graphics.FillEllipse(new SolidBrush(Color.FromArgb(128, Color.Red)), 110 - fontMaxSize / 2 - padding, 50 - fontMaxSize / 2 - padding, fontMaxSize + padding * 2, fontMaxSize + padding * 2);

                graphics.DrawString(item.DisplayValues[0], font, new SolidBrush(pluginSettings.FrontColor), 110, 50, stringFormat);
            }

            return bmp;
        }
        private string UpdateKeyTitle(Item item)
        {
            return item.UserId;
        }
        /// <summary>
        /// Google API 쿼리 클래스의 인스턴스를 가져옵니다.
        /// </summary>
        /// <returns></returns>
        private DataBinder GetApiInstance()
        {
            return dataBinder ?? new DataBinder(pluginSettings, item);
        }
        #endregion
    }
}
