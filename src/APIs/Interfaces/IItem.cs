using System.Collections.Generic;

namespace StreamDock.Plugin.GoogleAPI
{
    internal interface IItem
    {
        internal IList<string> DisplayValues { get; set; }
    }
}
