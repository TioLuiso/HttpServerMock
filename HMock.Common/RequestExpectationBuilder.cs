using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using HttpServerMock.Common.Model;

namespace HttpServerMock.Common
{
    public class RequestExpectationBuilder : IRequestExpectationBuilder
    {
        private string name;
        private Uri uri;
        private string regex;
        private string contentType;
        private Method method;
        private IDictionary<string, IEnumerable<string>> headers;
        private int numberOfCalls;
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
            this.headers = headers.ToDictionary(h => h.Key, h => new[] {h.Value}.Select(h2 => h2));
            return this;
        }

        public IRequestExpectationBuilder WithHeaders(IDictionary<string, IEnumerable<string>> headers)
        {
            this.headers = headers;
            return this;
        }

        public IRequestExpectationBuilder WithContent(IContent content)
        {
            this.content = content;
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
                return new RegexRequestExpectation(this.name, this.regex, this.contentType, this.method, this.headers, this.content, numberOfCalls, this.validator, this.responseBuilder);
            }

            if (this.uri != null && this.regex == null)
            {
                return new UrlRequestExpectation(this.name, this.uri, this.contentType, this.method, this.headers, this.content, numberOfCalls, this.validator, this.responseBuilder);
            }

            throw new InvalidOperationException("Exactly one of uri or regex must be set");
        }
    }
}