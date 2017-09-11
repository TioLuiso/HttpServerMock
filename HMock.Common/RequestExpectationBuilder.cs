using System;
using System.Collections.Generic;
using System.Linq;
using HttpServerMock.Common.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HttpServerMock.Common
{
    public class RequestExpectationBuilder : IRequestExpectationBuilder
    {
        private string name;
        private Uri uri;
        private string regex;
        private string contentType;
        private Method method;
        private IDictionary<string, List<string>> headers = new Dictionary<string, List<string>>();
        private int numberOfCalls = 1;
        private IExpectationResponseBuilder responseBuilder;
        private IContent content;
        private Func<Request, bool> validator = r => true;

        public IRequestExpectationBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public IRequestExpectationBuilder WithUri(Uri uri)
        {
            this.uri = uri;
            return this;
        }

        public IRequestExpectationBuilder WithUri(string uri)
        {
            this.uri = new Uri(uri);
            return this;
        }

        public IRequestExpectationBuilder WithRegex(string regex)
        {
            this.regex = regex;
            return this;
        }

        public IRequestExpectationBuilder WithContentType(string contentType)
        {
            this.contentType = contentType;
            return this;
        }

        public IRequestExpectationBuilder WithMethod(Method method)
        {
            this.method = method;
            return this;
        }

        public IRequestExpectationBuilder WithHeaders(IDictionary<string, string> headers)
        {
            this.headers = headers.ToDictionary(h => h.Key, h => new List<string> {h.Value});
            return this;
        }

        public IRequestExpectationBuilder WithHeader(string key, string value)
        {
            if (!this.headers.ContainsKey(key))
            {
                this.headers.Add(key, new List<string>());
            }

            this.headers[key].Add(value);

            return this;
        }

        public IRequestExpectationBuilder WithHeaders(IDictionary<string, IEnumerable<string>> headers)
        {
            this.headers = headers.ToDictionary(h => h.Key, h => h.Value.ToList());
            return this;
        }

        public IRequestExpectationBuilder WithContent(IContent content)
        {
            this.content = content;
            return this;
        }

        public IRequestExpectationBuilder WithStringContent(string content)
        {
            this.content = new StringContent(content);
            return this;
        }

        public IRequestExpectationBuilder WithBinaryContent(byte[] content)
        {
            this.content = new BinaryContent(content);
            return this;
        }

        public IRequestExpectationBuilder WithJsonContent(string content)
        {
            this.content = new JsonContent(JToken.Parse(content));
            return this;
        }

        public IRequestExpectationBuilder WithJsonContent(JToken content)
        {
            this.content = new JsonContent(content);
            return this;
        }

        public IRequestExpectationBuilder WithNumberOfCalls(int numberOfCalls)
        {
            this.numberOfCalls = numberOfCalls;
            return this;
        }

        public IRequestExpectationBuilder WithResponseBuilder(IExpectationResponseBuilder responseBuilder)
        {
            this.responseBuilder = responseBuilder;
            return this;
        }

        public IRequestExpectationBuilder WithValidator(Func<Request, bool> requestValidator)
        {
            this.validator = requestValidator;
            return this;
        }

        public IRequestExpectation Build()
        {
            if (this.regex != null && this.uri == null)
            {
                return new RegexRequestExpectation(this.name, this.regex, this.contentType, this.method, this.headers.ToDictionary(h => h.Key, h => h.Value.Select(v => v)), this.content, numberOfCalls, this.validator, this.responseBuilder);
            }

            if (this.uri != null && this.regex == null)
            {
                return new UrlRequestExpectation(this.name, this.uri, this.contentType, this.method, this.headers.ToDictionary(h => h.Key, h => h.Value.Select(v => v)), this.content, numberOfCalls, this.validator, this.responseBuilder);
            }

            throw new InvalidOperationException("Exactly one of uri or regex must be set");
        }
    }
}