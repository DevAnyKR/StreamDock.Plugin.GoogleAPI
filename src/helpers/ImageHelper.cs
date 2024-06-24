using BarRaider.SdTools;

using System.Drawing;

namespace StreamDock.Plugin
{
    public class ImageHelper
    {
        private static int maxWidth = 142;
        private static int maxHeight = 142;

        internal static Image GetImage(Color color)
        {
            var bmp = Tools.GenerateGenericKeyImage(out Graphics graphics);
            graphics.FillRectangle(new SolidBrush(color), 0, 0, bmp.Width, bmp.Height);

            return bmp;
        }

        internal static Font ResizeFont(Graphics graphics, string text, Font font)
        {
            var newSize = graphics.MeasureString(text, font);
            if (newSize.Width > maxWidth || newSize.Height > maxHeight)
            {
                return ResizeFont(graphics, text, new Font("Arial", font.Size - 2, FontStyle.Bold, GraphicsUnit.Pixel));
            }

            return font;
        }
        internal static Font ResizeFont(Graphics graphics, string text, Font font, int maxWidth, int maxHeight)
        {
            var newSize = graphics.MeasureString(text, font);
            if (newSize.Width > maxWidth || newSize.Height > maxHeight)
            {
                return ResizeFont(graphics, text, new Font("Arial", font.Size - 2, FontStyle.Bold, GraphicsUnit.Pixel));
            }

            return font;
        }

        internal static Image SetImageText(Image image, string text, Brush brush)
        {
            return SetImageText(image, text, brush, 72, 72);
        }

        internal static Image SetImageText(Image image, string text, Brush brush, int fontX, int fontY)
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
                graphics.DrawString(text, font, brush, !isRGB ? fontX : 5, fontY, stringFormat);
            }

            return image;
        }
    }
}
