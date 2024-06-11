using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;

namespace StreamDock.Plugin.GoogleAPIs
{
    /// <summary>
    /// API 최상위 클래스
    /// </summary>
    internal abstract class GoogleAPIs
    {
        public GoogleAPIs() {
            GoogleWebAuthorizationBroker.Folder = "StreamDock Plugin";
        }
    }
}
