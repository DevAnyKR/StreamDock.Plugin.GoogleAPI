using BarRaider.SdTools;

namespace StreamDock.Plugins
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
