using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HttpServerMock.Common.Model;

namespace HttpServerMock.Common
{
    public class RegexRequestExpectation : BaseRequestExpectation
    {
        public RegexRequestExpectation(string name, string regex, string contentType, Method method, IDictionary<string, IEnumerable<string>> headers, IContent content, int numberOfCalls, Func<Request, bool> validator, IExpectationResponseBuilder responseBuilder)
            : base(name, contentType, method, headers, content, numberOfCalls, validator, responseBuilder)
        {
            this.Regex = new Regex(regex);
        }

        public Regex Regex { get; }

        protected override bool InnerValidate(Request request)
        {
            return this.Regex.IsMatch(request.Uri.PathAndQuery);
        }
    }
}