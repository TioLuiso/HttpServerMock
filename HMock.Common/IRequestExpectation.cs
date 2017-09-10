﻿using System.Collections.Generic;
using System.Threading.Tasks;
using HttpServerMock.Common.Model;

namespace HttpServerMock.Common
{
    public interface IRequestExpectation
    {
        string ContentType { get; }
        Method Method { get; }
        IDictionary<string, IEnumerable<string>> RequestHeaders { get; }
        IContent Content { get; }
        IExpectationResponseBuilder ResponseBuilder { get; }
        int NumberOfCalls { get; }

        bool CanProcessRequest(Request request);
        Task<Response> ProcessRequest(Request request);
    }
}