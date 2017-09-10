using System;
using System.Collections.Generic;

namespace HttpServerMock.Common.Model
{
    public class Request
    {
        public Method Method { get; set; }
        public Uri Uri { get; set; }
        public IDictionary<string, IEnumerable<string>> Headers { get; } = new Dictionary<string, IEnumerable<string>>();
        public IEnumerable<string> ContentType => this.Headers["Content-Type"];
        public IContent Content { get; set; }
    }
}