using System;
using Microsoft.AspNet.SignalR;
using NzbDrone.Common.Composition;
using NzbDrone.Common.EnvironmentInfo;
using NzbDrone.SignalR;
using Owin;

namespace NzbDrone.Host.Owin.MiddleWare
{
    public class SignalRMiddleWare : IOwinMiddleWare
    {
        public int Order => 1;

        public SignalRMiddleWare(IContainer container)
        {
            SignalRDependencyResolver.Register(container);
            SignalRJsonSerializer.Register();

            // Half the default time (110s) to get under nginx's default 60 proxy_read_timeout
            GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(55);
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromMinutes(3);
        }

        public void Attach(IAppBuilder appBuilder)
        {
            appBuilder.MapSignalR("/signalr", typeof(NzbDronePersistentConnection), new ConnectionConfiguration());
        }
    }
}
