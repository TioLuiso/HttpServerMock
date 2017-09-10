using System.Collections.Generic;
using System.Threading.Tasks;
using HttpServerMock.Common.Model;

namespace HttpServerMock.Common
{
    public class ExpectationResponseBuilder : IExpectationResponseBuilder
    {
        private StatusCode statusCode;

        private IDictionary<string, IEnumerable<string>> headers = new Dictionary<string, IEnumerable<string>>();

        private IContent content;

        private int responseTime;

        public IExpectationResponseBuilder WithStatusCode(StatusCode statusCode)
        {
            this.statusCode = statusCode;
            return this;
        }

        public IExpectationResponseBuilder WithHeaders(IDictionary<string, IEnumerable<string>> headers)
        {
            this.headers = headers;
            return this;
        }

        public IExpectationResponseBuilder WithContent(IContent content)
        {
            this.content = content;
            return this;
        }

        public IExpectationResponseBuilder WithResponseTime(int milliseconds)
        {
            this.responseTime = milliseconds;
            return this;
        }

        public async Task<Response> BuildAsync(Request request)
        {
            await Task.Delay(this.responseTime);
            return new Response(this.statusCode, this.headers, this.content);
        }

    }
}