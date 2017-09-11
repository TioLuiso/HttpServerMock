using HttpServerMock.Common;
using Microsoft.Owin.Hosting;

namespace HttpServerMock
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Web.Http.SelfHost;

    /// <summary>
    /// HTTP server mock class.
    /// </summary>
    public sealed class HttpServerMock : IDisposable
    {
        private bool isDisposed;
        private IDisposable webapp;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServerMock"/> class.
        /// </summary>
        /// <param name="port">The HTTP server port.</param>
        public HttpServerMock(uint port)
        {
            this.IpOrDns = Dns.GetHostName();
            this.Port = port;
            this.ServerRequestsState = new ServerRequestsState();

            this.StartServer();
        }

        /// <summary>
        /// Gets the HTTP server IP or DNS.
        /// </summary>
        public string IpOrDns { get; private set; }

        /// <summary>
        /// Gets the HTTP server port.
        /// </summary>
        public uint Port { get; private set; }

        /// <summary>
        /// Gets the current server requests state.
        /// </summary>
        public ServerRequestsState ServerRequestsState { get; private set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!this.isDisposed)
            {
                this.webapp.Dispose();
                this.isDisposed = true;
            }
        }

        private void StartServer()
        {
            string baseAddress = string.Format(CultureInfo.InvariantCulture, "http://{0}:{1}", this.IpOrDns, this.Port);

            var options = new StartOptions(baseAddress);
            var requestMapper = new RequestMapper();
            var requestProcessor = new RequestProcessor(this.ServerRequestsState);
            var responseMapper = new ResponseMapper();
            var startup = new Startup(requestMapper, responseMapper, requestProcessor);

            this.webapp = WebApp.Start(options, builder => startup.Configuration(builder));
        }
    }
}