using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamDock.Plugin.GoogleAPIs.AdSenseManagement
{
    public class Item
    {
        internal bool DataReceived {  get; set; }
        internal bool DataValid { get; set; }
        internal string Value1 { get; set; }
        internal string Value2 { get; set; }

        internal static Item Instance { get; set; } = new Item();
        internal Item()
        {
            Init();
        }

        internal void Init()
        {
            DataReceived = false;
            DataValid = false;
            Value1 = null;
            Value2 = null;
        }
    }
}
