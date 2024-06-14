using System.Drawing;

namespace StreamDock.Plugins
{
    internal class KeyImage
    {
        internal Image Init { get; set; }
        internal Image KeyPressed { get; set; }
        internal Image KeyReleased { get; set; } = null;
        internal Image ActionProcessing { get; set;}
        internal Image Final { get; set;}
    }
}
