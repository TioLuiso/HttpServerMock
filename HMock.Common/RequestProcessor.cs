using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpServerMock.Common.Model;

namespace HttpServerMock.Common
{
    public sealed class RequestProcessor : IRequestProcessor
    {
        private readonly ServerRequestsState serverRequestsState;

        public RequestProcessor(ServerRequestsState serverRequestsState)
        {
            this.serverRequestsState = serverRequestsState;
        }

        public async Task<Response> SendAsync(Request request)
        {
            IRequestExpectation expectation = this.FindExpectationForRequest(request);

            if (expectation != null)
            {
                return await expectation.ProcessRequest(request);
            }

            return this.AddUnexpectedRequest(request);
        }

        private IRequestExpectation FindExpectationForRequest(Request request)
        {
            return this.serverRequestsState.RequestExpectations.FirstOrDefault(e => e.CanProcessRequest(request));
        }

        private Response AddUnexpectedRequest(Request request)
        {
            this.serverRequestsState.UnexpectedRequests.Add(request);

            return new Response(this.serverRequestsState.DefaultRespondStatusCode, new Dictionary<string, IEnumerable<string>>(), null);
        }
    }
}