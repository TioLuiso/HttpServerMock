using System.Collections.Generic;
using System.Threading.Tasks;
using HttpServerMock.Common.Model;

namespace HttpServerMock.Common
{
    public interface IExpectationResponseBuilder
    {
        IExpectationResponseBuilder WithStatusCode(StatusCode statusCode);
        IExpectationResponseBuilder WithHeaders(IDictionary<string, IEnumerable<string>> headers);
        IExpectationResponseBuilder WithContent(IContent content);
        IExpectationResponseBuilder WithResponseTime(int milliseconds);
        Task<Response> BuildAsync(Request request);
    }
}