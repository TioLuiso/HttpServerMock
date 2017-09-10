using System.Threading.Tasks;
using HttpServerMock.Common;
using HttpServerMock.Common.Model;
using Microsoft.Owin;

namespace HttpServerMock
{
    /// <summary>
    /// Owin middleware that processes requests and returns expected responses
    /// </summary>
    public class MockProcessor : OwinMiddleware
    {
        private readonly IRequestMapper requestMapper;
        private readonly IResponseMapper responseMapper;
        private readonly IRequestProcessor requestProcessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="requestMapper"></param>
        /// <param name="responseMapper"></param>
        /// <param name="requestProcessor"></param>
        public MockProcessor(
            OwinMiddleware next,
            IRequestMapper requestMapper,
            IResponseMapper responseMapper,
            IRequestProcessor requestProcessor) : base(next)
        {
            this.requestMapper = requestMapper;
            this.responseMapper = responseMapper;
            this.requestProcessor = requestProcessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task Invoke(IOwinContext context)
        {
            Request request = this.requestMapper.Map(context.Request);
            Response response = await this.requestProcessor.SendAsync(request);
            this.responseMapper.MapResponse(response, context);
        }
    }
}