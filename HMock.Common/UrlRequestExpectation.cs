using System;
using System.Collections.Generic;
using HttpServerMock.Common.Model;

namespace HttpServerMock.Common
{
    public class UrlRequestExpectation : BaseRequestExpectation
    {
        public UrlRequestExpectation(string name, Uri uri, string contentType, Method method, IDictionary<string, IEnumerable<string>> headers, IContent content, int numberOfCalls, Func<Request, bool> validator, IExpectationResponseBuilder responseBuilder)
            : base(name, contentType, method, headers, content, numberOfCalls, validator, responseBuilder)
        {
            this.Uri = uri;
        }

        public Uri Uri { get; }

        protected override bool InnerValidate(Request request)
        {
            // https://msdn.microsoft.com/en-us/library/system.uri.pathandquery(v=vs.110).aspx
            var comparisonResult = Uri.Compare(
                this.Uri,
                request.Uri,
                UriComponents.PathAndQuery,
                UriFormat.SafeUnescaped,
                StringComparison.OrdinalIgnoreCase);

            if (comparisonResult != 0)
            {
                return false;
            }

            // https://msdn.microsoft.com/en-us/library/system.uri.userinfo(v=vs.110).aspx
            comparisonResult = Uri.Compare(
                this.Uri,
                request.Uri,
                UriComponents.UserInfo,
                UriFormat.SafeUnescaped,
                StringComparison.OrdinalIgnoreCase);

            if (comparisonResult != 0)
            {
                return false;
            }

            // https://msdn.microsoft.com/en-us/library/system.uri.fragment(v=vs.110).aspx
            comparisonResult = Uri.Compare(
                this.Uri,
                request.Uri,
                UriComponents.Fragment,
                UriFormat.SafeUnescaped,
                StringComparison.OrdinalIgnoreCase);

            if (comparisonResult != 0)
            {
                return false;
            }

            return true;
        }
    }
}