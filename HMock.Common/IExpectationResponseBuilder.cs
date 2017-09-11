using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using HttpServerMock.Common.Model;
using Newtonsoft.Json.Linq;

namespace HttpServerMock.Common
{
    public interface IExpectationResponseBuilder
    {
        IExpectationResponseBuilder WithStatusCode(StatusCode statusCode);
        IExpectationResponseBuilder WithHeaders(IDictionary<string, IEnumerable<string>> headers);
        IExpectationResponseBuilder WithHeaders(IDictionary<string, string> headers);
        IExpectationResponseBuilder WithHeader(string key, string value);
        IExpectationResponseBuilder WithContentTypes(IEnumerable<string> contentTypes);
        IExpectationResponseBuilder WithContentType(string contentType);
        IExpectationResponseBuilder WithContent(IContent content);
        IExpectationResponseBuilder WithStringContent(string content);
        IExpectationResponseBuilder WithBinaryContent(byte[] content);
        IExpectationResponseBuilder WithJsonContent(string content);
        IExpectationResponseBuilder WithJsonContent(JToken content);
        IExpectationResponseBuilder WithJsonContent<T>(T content);
        IExpectationResponseBuilder WithXmlContent(string content);
        IExpectationResponseBuilder WithXmlContent(XElement content);
        IExpectationResponseBuilder WithXmlContent<T>(T content);
        IExpectationResponseBuilder WithResponseTime(int milliseconds);
        Task<Response> BuildAsync(Request request);
    }
}