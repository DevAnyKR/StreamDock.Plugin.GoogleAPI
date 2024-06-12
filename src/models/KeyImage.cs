using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamDock.Plugin
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
