using System;

namespace StreamDock.Plugin.GoogleAPI
{
    internal interface IPluginSettings
    {
        internal TimeSpan RefreshIntervalMin { get; }
    }
}
