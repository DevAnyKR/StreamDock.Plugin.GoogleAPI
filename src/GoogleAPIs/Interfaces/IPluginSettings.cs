using System;

namespace StreamDock.Plugins.GoogleAPIs
{
    internal interface IPluginSettings
    {
        internal TimeSpan RefreshIntervalMin { get; }
    }
}
