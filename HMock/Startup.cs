using HttpServerMock.Common;
using Owin;

namespace HttpServerMock
{
    /// <summary>
    /// Configuration for Owin application
    /// </summary>
    public class Startup
    {
        private readonly IRequestMapper requestMapper;
        private readonly IResponseMapper responseMapper;
        private readonly IRequestProcessor requestProcessor;

        /// <summary>
        /// 
        /// </summary>
        public Startup(IRequestMapper requestMapper, IResponseMapper responseMapper, IRequestProcessor requestProcessor)
        {
            this.requestMapper = requestMapper;
            this.responseMapper = responseMapper;
            this.requestProcessor = requestProcessor;
        }

        /// <summary>
        /// Configuration of Owin application
        /// </summary>
        /// <param name="builder">App builder</param>
        public void Configuration(IAppBuilder builder)
        {
            builder.Use<MockProcessor>(this.requestMapper, this.responseMapper, this.requestProcessor);
        }
    }
}