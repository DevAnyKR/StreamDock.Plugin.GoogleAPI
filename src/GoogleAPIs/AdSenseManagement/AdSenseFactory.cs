using System;

using BarRaider.SdTools;

namespace StreamDock.Plugin.GoogleAPIs.AdSenseManagement
{
    internal static class AdSenseFactory
    {
        internal static AdSense GetAdSenseInstance(ISDConnection connection, PluginSettings adSenseSettings)
        {
            switch (adSenseSettings.ViewType)
            {
                case ViewType.Payments:
                    return new Payment(connection, adSenseSettings.ViewType);
                case ViewType.Today:
                case ViewType.Monthly:
                default:
                    throw new NotImplementedException();
            }
        } 
    }
}
