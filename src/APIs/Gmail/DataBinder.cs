using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace StreamDock.Plugin.GoogleAPI.Gmail
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
        internal async Task ServiceExecuteAsync()
        {
            ApiService apiSevice = await ApiService.GetInstance(googleAuth.userCredential, pluginSettings.UserTokenName);

            item.UserId = apiSevice.GetUserId();
            item.MessageUnReadCount = apiSevice.GetResultSizeEstimate(item.UserId);

            SetDisplayValue();
        }
        internal void SetInitialValue()
        {
            item.Init();
            item.DisplayValues.OnlyOne("0");
        }
        internal void SetDisplayValue()
        {
            item.DisplayValues.Clear();
            item.DisplayValues.Add(item.MessageUnReadCount.ToString());
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