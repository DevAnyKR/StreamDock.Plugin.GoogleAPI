using BarRaider.SdTools;

using System.Drawing;

namespace StreamDock.Plugin.GoogleAPIs
{
    public static class ImageHelper
    {
        internal static Image GetImage(Color color)
        {
            var bmp = Tools.GenerateGenericKeyImage(out Graphics graphics);
            graphics.FillRectangle(new SolidBrush(color), 0, 0, bmp.Width, bmp.Height);

            return bmp;
        }

        internal static Font ResizeFont(Graphics graphics, string text, Font font)
        {
            var newSize = graphics.MeasureString(text, font);
            if (newSize.Width > 142 || newSize.Height > 142)
            {
                return ResizeFont(graphics, text, new Font("Arial", font.Size - 2, FontStyle.Bold, GraphicsUnit.Pixel));
            }

            return font;
        }

        internal static Image SetImageText(Image image, string text)
        {
            var font = new Font("Arial", 70, FontStyle.Bold, GraphicsUnit.Pixel);
            var stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            var isRGB = stringFormat.Alignment == StringAlignment.Near;

            using (var graphics = Graphics.FromImage(image))
            {
                font = ResizeFont(graphics, text, font);
                graphics.DrawString(text, font, Brushes.White, !isRGB ? 72 : 5, 72, stringFormat);
            }

            return image;
        }
    }
}
