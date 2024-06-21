using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing;
using System.Threading.Tasks;

using Google.Apis.Gmail.v1;
using Google.Apis.Oauth2.v2;
using System.Collections.Generic;

namespace StreamDock.Plugins.GoogleAPIs.Gmail
{
    internal class DataBinder : DataModel<PluginSettings, Item>
    {
        internal bool ExistsUserCredential => GoogleAuth.CredentialExist(pluginSettings.UserTokenName);
        internal DataBinder(PluginSettings pluginSettings)
        {
            this.pluginSettings = pluginSettings;
            this.item = new();
            this.googleAuth = new();
        }
        internal async Task<Item> ServiceExecuteAsync()
        {
            Oauth2Service oauth2Service = new Oauth2Service(googleAuth.GetServiceInitializerAsync(pluginSettings.UserTokenName).Result);
            item.UserId = oauth2Service.Userinfo.Get().Execute().Email;

            ApiService apiSevice = new ApiService(googleAuth.userCredential, new GmailService(await googleAuth.GetServiceInitializerAsync(pluginSettings.UserTokenName)));

            var request = await apiSevice.GetMessagesListAsync(item.UserId, "UNREAD");

            item.MessageUnReadCount = request.ResultSizeEstimate;

            item = SetDisplayValue();

            return item;
        }
        internal void SetInitialValue()
        {
            item.DisplayValues.OnlyOne(WaitingMessage);
        }
        internal Item SetDisplayValue()
        {
            item.DisplayValues.Clear();
            item.DisplayValues.Add(item.MessageUnReadCount.ToString());

            return item;
        }
        internal string GetDisplayTitle()
        {
            return item.UserId;
        }
        internal bool CheckExistData()
        {
            return item.MessageUnReadCount > 0;
        }
        internal Bitmap GetUpdateKeyImage()
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

                font = ImageHelper.ResizeFont(graphics, item.DisplayValues[0], font, 50, 50);

                // 동그라미. 중심을 기준으로 그릴 것. 글꼴 크기 자동 축소
                var circleSizeX = 60;
                var circleSizeY = circleSizeX;
                var circleCenterPosX = 110;
                var circleCenterPosY = 35;
                graphics.FillEllipse(new SolidBrush(Color.FromArgb(128, pluginSettings.CircleColor)), circleCenterPosX - circleSizeX / 2, circleCenterPosY - circleSizeY / 2, circleSizeX, circleSizeY);

                graphics.DrawString(item.DisplayValues[0], font, new SolidBrush(pluginSettings.FrontColor), circleCenterPosX, circleCenterPosY, stringFormat);
            }

            return bmp;
        }
    }
}