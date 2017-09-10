using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using HttpServerMock.Common.Model;

namespace HttpServerMock.Common
{
    /// <summary>
    /// Contains the Http mock server requests state.
    /// </summary>
    public sealed class ServerRequestsState
    {
        private readonly List<IRequestExpectation> requestExpectations = new List<IRequestExpectation>();

        /// <summary>
        /// Gets the request expectations configured for the Http server mock.
        /// </summary>
        public IReadOnlyCollection<IRequestExpectation> RequestExpectations => this.requestExpectations;

        /// <summary>
        /// Gets the unexpected requests which were managed by the Http server mock.
        /// </summary>
        public ICollection<Request> UnexpectedRequests { get; } = new List<Request>();

        /// <summary>
        /// Gets or sets the respond status code which will be returned by the server for those
        /// request which has not any expectation or behavior.
        /// </summary>
        public StatusCode DefaultRespondStatusCode { get; } = StatusCode.NotImplemented;

        public void AddExpectaction(IRequestExpectation expectation)
        {
            this.requestExpectations.Add(expectation);
        }
    }
}