using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpServerMock.Common.Model;
using Newtonsoft.Json.Linq;

namespace HttpServerMock.Common
{
    public class ExpectationResponseBuilder : IExpectationResponseBuilder
    {
        private StatusCode statusCode;

        private IDictionary<string, List<string>> headers = new Dictionary<string, List<string>>();

        private IContent content;

        private int responseTime;

        public IExpectationResponseBuilder WithStatusCode(StatusCode statusCode)
        {
            this.statusCode = statusCode;
            return this;
        }

        public IExpectationResponseBuilder WithHeaders(IDictionary<string, IEnumerable<string>> headers)
        {
            this.headers = headers.ToDictionary(h => h.Key, h => h.Value.ToList());
            return this;
        }

        public IExpectationResponseBuilder WithHeaders(IDictionary<string, string> headers)
        {
            this.headers = headers.ToDictionary(h => h.Key, h => new List<string> { h.Value });
            return this;
        }

        public IExpectationResponseBuilder WithHeader(string key, string value)
        {
            if (!this.headers.ContainsKey(key))
            {
                this.headers.Add(key, new List<string>());
            }

            this.headers[key].Add(value);

            return this;
        }

        public IExpectationResponseBuilder WithContentTypes(IEnumerable<string> contentTypes)
        {
            foreach (var contentType in contentTypes)
            {
                this.WithContentType(contentType);
            }

            return this;
        }

        public IExpectationResponseBuilder WithContentType(string contentType)
        {
            return this.WithHeader("Content-Type", contentType);
        }

        public IExpectationResponseBuilder WithContent(IContent content)
        {
            this.content = content;
            return this;
        }

        public IExpectationResponseBuilder WithStringContent(string content)
        {
            this.content = new StringContent(content);
            return this;
        }

        public IExpectationResponseBuilder WithBinaryContent(byte[] content)
        {
            this.content = new BinaryContent(content);
            return this;
        }

        public IExpectationResponseBuilder WithJsonContent(string content)
        {
            this.content = new JsonContent(JToken.Parse(content));
            return this;
        }

        public IExpectationResponseBuilder WithJsonContent(JToken content)
        {
            this.content = new JsonContent(content);
            return this;
        }

        public IExpectationResponseBuilder WithJsonContent<T>(T content)
        {
            JToken tokenContent = JToken.FromObject(content);
            this.content = new JsonContent(tokenContent);
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
            return new Response(this.statusCode, this.headers.ToDictionary(h => h.Key, h => h.Value.Select(v => v)), this.content);
        }

    }
}