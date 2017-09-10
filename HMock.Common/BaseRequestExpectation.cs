using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpServerMock.Common.Model;

namespace HttpServerMock.Common
{
    public abstract class BaseRequestExpectation : IRequestExpectation
    {
        private int doneCalls;

        protected BaseRequestExpectation(string name, string contentType, Method method, IDictionary<string, IEnumerable<string>> headers, IContent content, int numberOfCalls, Func<Request, bool> validator, IExpectationResponseBuilder responseBuilder)
        {
            Name = name;
            ContentType = contentType;
            Method = method;
            this.RequestHeaders = headers;
            Content = content;
            NumberOfCalls = numberOfCalls;
            Validator = validator;
            ResponseBuilder = responseBuilder;
        }

        public string Name { get; }
        public string ContentType { get; }
        public Method Method { get; }
        public IDictionary<string, IEnumerable<string>> RequestHeaders { get; }
        public IContent Content { get; }
        public IExpectationResponseBuilder ResponseBuilder { get; }
        public int NumberOfCalls { get; }
        public Func<Request, bool> Validator { get; }

        public bool CanProcessRequest(Request request)
        {
            return ValidatePendingCalls()
                && this.Validator(request)
                && InnerValidate(request)
                && ValidateContentType(request)
                && ValidateMethod(request)
                && ValidateHeaders(request)
                && ValidateContent(request);
        }

        public Task<Response> ProcessRequest(Request request)
        {
            this.doneCalls++;
            return this.ResponseBuilder.BuildAsync(request);
        }

        protected abstract bool InnerValidate(Request request);

        private bool ValidateContentType(Request request)
        {
            return request.ContentType.Contains(this.ContentType);
        }

        private bool ValidateMethod(Request request)
        {
            return request.Method == this.Method;
        }

        private bool ValidateHeaders(Request request)
        {
            if (this.RequestHeaders != null)
            {
                foreach (var expectedHeaderKey in this.RequestHeaders.Keys)
                {
                    if (!request.Headers.TryGetValue(expectedHeaderKey, out var requestHeader))
                    {
                        var expectedHeader = this.RequestHeaders[expectedHeaderKey];
                        if (expectedHeader.Except(requestHeader).Any())
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool ValidateContent(Request request)
        {
            if (this.Content == null)
            {
                return true;
            }

            return this.Content.Equals(request.Content);
        }

        private bool ValidatePendingCalls()
        {
            return this.NumberOfCalls > this.doneCalls;
        }
    }
}