﻿namespace StreamDock.Plugin.GoogleAPI.AdSenseManagement
{
    /// <summary>
    /// manifest.json 에서 선언한 플러그인 UUID
    /// </summary>
    [PluginActionId("kr.devany.googleapi.adsensemanagement")]
    public class PluginAction : KeypadBase
    {
        InitialPayload initialPayload;
        PluginService pluginService;
        DataBinder dataBinder;

        public PluginAction(ISDConnection connection, InitialPayload payload) : base(connection, payload)
        {
            this.initialPayload = payload;
            pluginService = new();
            dataBinder = new((payload.Settings == null || payload.Settings.Count == 0) ? new PluginSettings() : payload.Settings.ToObject<PluginSettings>());

            Connection.OnApplicationDidLaunch += Connection_OnApplicationDidLaunch;
            Connection.OnApplicationDidTerminate += Connection_OnApplicationDidTerminate;
            Connection.OnDeviceDidConnect += Connection_OnDeviceDidConnect;
            Connection.OnDeviceDidDisconnect += Connection_OnDeviceDidDisconnect;
            Connection.OnPropertyInspectorDidAppear += Connection_OnPropertyInspectorDidAppear;
            Connection.OnPropertyInspectorDidDisappear += Connection_OnPropertyInspectorDidDisappear;
            Connection.OnSendToPlugin += Connection_OnSendToPlugin;
            Connection.OnTitleParametersDidChange += Connection_OnTitleParametersDidChange;
            dataBinder.pluginSettings.PropertyChanged += PropertyChanged;
        }
        #region Event
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
                Logger.Instance.LogMessage(TracingLevel.INFO, $"[{initialPayload.Coordinates.Row},{initialPayload.Coordinates.Column}] OnTitleParametersDidChange Event Handled");
                if (!pluginService.HasExecuteOnce)
                {
                    if (!dataBinder.ExistsUserCredential)
                    {
                        await DisplayInitialAsync();
                    }
                    else
                    {
                        await DisplayBusyAsync();
                        await UpdateApiDataAsync();
                    }
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
            Logger.Instance.LogMessage(TracingLevel.INFO, $"[{initialPayload.Coordinates.Row},{initialPayload.Coordinates.Column}] OnSendToPlugin Event Handled");
        }

        /// <summary>
        /// PropertyInspector가 표시될 때마다 호출됩니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnPropertyInspectorDidAppear(object sender, SDEventReceivedEventArgs<BarRaider.SdTools.Events.PropertyInspectorDidAppear> e)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, $"[{initialPayload.Coordinates.Row},{initialPayload.Coordinates.Column}] OnPropertyInspectorDidAppear Event Handled");
        }

        /// <summary>
        /// PropertyInspector가 숨겨질 때마다 호출됩니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnPropertyInspectorDidDisappear(object sender, SDEventReceivedEventArgs<BarRaider.SdTools.Events.PropertyInspectorDidDisappear> e)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, $"[{initialPayload.Coordinates.Row},{initialPayload.Coordinates.Column}] OnPropertyInspectorDidDisappear Event Handled");
        }


        /// <summary>
        /// 장치 연결 끊김 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnDeviceDidDisconnect(object sender, SDEventReceivedEventArgs<BarRaider.SdTools.Events.DeviceDidDisconnect> e)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, $"[{initialPayload.Coordinates.Row},{initialPayload.Coordinates.Column}] OnDeviceDidDisconnect Event Handled");
        }

        /// <summary>
        /// 장치 연결 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnDeviceDidConnect(object sender, SDEventReceivedEventArgs<BarRaider.SdTools.Events.DeviceDidConnect> e)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, $"[{initialPayload.Coordinates.Row},{initialPayload.Coordinates.Column}] OnDeviceDidConnect Event Handled");
        }

        /// <summary>
        /// 앱 종료 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnApplicationDidTerminate(object sender, SDEventReceivedEventArgs<BarRaider.SdTools.Events.ApplicationDidTerminate> e)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, $"[{initialPayload.Coordinates.Row},{initialPayload.Coordinates.Column}] OnApplicationDidTerminate Event Handled");
        }

        /// <summary>
        /// 앱 실행 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_OnApplicationDidLaunch(object sender, SDEventReceivedEventArgs<BarRaider.SdTools.Events.ApplicationDidLaunch> e)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, $"[{initialPayload.Coordinates.Row},{initialPayload.Coordinates.Column}] OnApplicationDidLaunch Event Handled");
        }
        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, $"[{initialPayload.Coordinates.Row},{initialPayload.Coordinates.Column}] {e.PropertyName} Property Changed");
        }
        #endregion
        #region Steam Dock
        /// <summary>
        /// 스트림독에 플러그인이 표시되지 않으면 호출됩니다.
        /// </summary>
        public override void Dispose()
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, $"[{initialPayload.Coordinates.Row},{initialPayload.Coordinates.Column}] Destructor called");
        }

        /// <summary>
        /// 키를 눌렀을 때 호출됩니다.
        /// </summary>
        /// <param name="payload"></param>
        public override void KeyPressed(KeyPayload payload)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, $"[{initialPayload.Coordinates.Row},{initialPayload.Coordinates.Column}] KeyPressed called");
        }

        /// <summary>
        /// 키를 뗄 때 호출됩니다.
        /// </summary>
        /// <param name="payload"></param>
        public async override void KeyReleased(KeyPayload payload)
        {
            try
            {
                Logger.Instance.LogMessage(TracingLevel.INFO, $"[{initialPayload.Coordinates.Row},{initialPayload.Coordinates.Column}] KeyReleased called");
                await DisplayBusyAsync();
                await UpdateApiDataAsync();
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
        public async override void OnTick()
        {
            try
            {
                if (pluginService.IsRefreshable(dataBinder.pluginSettings.RefreshIntervalMin))
                {
                    Logger.Instance.LogMessage(TracingLevel.INFO, $"[{initialPayload.Coordinates.Row},{initialPayload.Coordinates.Column}] Refresh...");
                    if (dataBinder.ExistsUserCredential)
                    {
                        await UpdateApiDataAsync();
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// PropertyInspector에서 값이 변경될 때마다 호출됩니다.
        /// </summary>
        /// <param name="payload"></param>
        public async override void ReceivedSettings(ReceivedSettingsPayload payload)
        {
            try
            {
                Logger.Instance.LogMessage(TracingLevel.INFO, $"[{initialPayload.Coordinates.Row},{initialPayload.Coordinates.Column}] ReceivedSettings called");

                Tools.AutoPopulateSettings(dataBinder.pluginSettings, payload.Settings);
                //await SaveSettingsAsync(); // 스트림독으로 설정 업로드

                await DisplayInitialAsync();
                if (dataBinder.ExistsUserCredential)
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

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, $"[{initialPayload.Coordinates.Row},{initialPayload.Coordinates.Column}] ReceivedGlobalSettings called");
            Tools.AutoPopulateSettings(dataBinder.pluginSettings, payload.Settings);
        }
        #endregion
        #region Private Methods
        /// <summary>
        /// 설정 값을 스트림독으로 전달합니다.
        /// </summary>
        /// <returns></returns>
        private async Task SaveSettingsAsync()
        {
            await Connection.SetSettingsAsync(JObject.FromObject(dataBinder.pluginSettings));
        }
        /// <summary>
        /// 초기 이미지를 표시합니다.
        /// </summary>
        private async Task DisplayInitialAsync()
        {
            dataBinder.SetInitialValue();
            await Connection.SetImageAsync(dataBinder.GetUpdateKeyImage(true)); // 초기 이미지 출력
        }
        private async Task DisplayPreValueAsync()
        {
            UpdateValues();
            await Connection.SetImageAsync(dataBinder.GetUpdateKeyImage(true)); // 초기 이미지 출력
        }
        /// <summary>
        /// 작업 중임을 알리는 이미지를 표시합니다.
        /// </summary>
        private async Task DisplayBusyAsync()
        {
            await Connection.SetImageAsync(dataBinder.GetLoadingKeyImage());
        }
        /// <summary>
        /// PI 설정에 따라 Google API 데이터를 갱신합니다.
        /// </summary>
        private async Task UpdateApiDataAsync()
        {
            await dataBinder.ServiceExecuteAsync();
            Logger.Instance.LogMessage(TracingLevel.INFO, $"[{initialPayload.Coordinates.Row},{initialPayload.Coordinates.Column}] UpdateApiDataAsync: Sending Image to Stream Dock...");
            await Connection.SetImageAsync(dataBinder.GetUpdateKeyImage());
            pluginService.SetExecuted();
            pluginService.UpdateRefreshTime();
        }
        /// <summary>
        /// PI 설정에 따라 이미 수신된 Google API 데이터로 갱신합니다.
        /// </summary>
        private void UpdateValues()
        {
            dataBinder.SetDisplayValue();
        }
        #endregion
    }
}
