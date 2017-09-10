using System;
using System.Collections.Generic;
using HttpServerMock.Common.Model;

namespace HttpServerMock.Common
{
    public interface IRequestExpectationBuilder
    {
        IRequestExpectationBuilder WithName(string name);
        IRequestExpectationBuilder WithUri(Uri uri);
        IRequestExpectationBuilder WithUri(string uri);
        IRequestExpectationBuilder WithRegex(string regex);
        IRequestExpectationBuilder WithContentType(string contentType);
        IRequestExpectationBuilder WithMethod(Method method);
        IRequestExpectationBuilder WithHeaders(IDictionary<string, IEnumerable<string>> headers);
        IRequestExpectationBuilder WithHeaders(IDictionary<string, string> headers);
        IRequestExpectationBuilder WithContent(IContent content);
        IRequestExpectationBuilder WithNumberOfCalls(int numberOfCalls);
        IRequestExpectationBuilder WithResponseBuilder(IExpectationResponseBuilder responseBuilder);
        IRequestExpectationBuilder WithValidator(Func<Request, bool> requestValidator);
        IRequestExpectation Build();
    }
}