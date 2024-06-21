using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarRaider.SdTools.Wrappers;

using BarRaider.SdTools;

namespace StreamDock.Plugins.GoogleAPIs
{
    internal abstract class DataModel<T1, T2>
    {
        internal T1 pluginSettings { get; set; }
        internal T2 item { get; set; }
        internal GoogleAuth googleAuth;

        /// <summary>
        /// 정보를 출력하기 전에 표시되는 로딩중 이미지입니다.
        /// </summary>
        /// <returns></returns>
        internal virtual Bitmap GetLoadingKeyImage()
        {
            TitleParameters tp = new TitleParameters(new FontFamily("Arial"), FontStyle.Bold, 12, Color.White, true, TitleVerticalAlignment.Middle);
            Bitmap image = Tools.GenerateGenericKeyImage(out Graphics graphics);
            graphics.FillRectangle(new SolidBrush(Color.Yellow), 0, 0, image.Width, image.Height);
            graphics.AddTextPath(tp, image.Height, image.Width, "Loading...", Color.Black, 7); //TODO 지역화
            graphics.Dispose();
            return image;
        }
    }
}
