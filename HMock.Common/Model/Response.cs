using System.Collections;
using System.Collections.Generic;

namespace HttpServerMock.Common.Model
{
    public class Response
    {
        public Response(StatusCode statusCode, IDictionary<string, IEnumerable<string>> headers, IContent content)
        {
            StatusCode = statusCode;
            Headers = headers;
            Content = content;
        }

        public StatusCode StatusCode { get; }

        public IDictionary<string, IEnumerable<string>> Headers { get; }

        public IContent Content { get; }
    }
}