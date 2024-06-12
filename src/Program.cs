using System;

using BarRaider.SdTools;

namespace StreamDock.Plugin
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "Main called");
            SDWrapper.Run(args);
        }
    }
}
