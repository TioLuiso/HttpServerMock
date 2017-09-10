using HttpServerMock.Common;
using HttpServerMock.Common.Model;

namespace HttpServerMock.ExtensionMethods
{

    using global::HttpServerMock.Exceptions;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using Method = global::HttpServerMock.Common.Model.Method;

    /// <summary>
    /// Extension methods for HttpServerMock class.
    /// </summary>
    public static class HttpServerMockExtensions
    {
        /// <summary>
        /// Sets up get expectation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serverMock">The server.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="requetsUri">The requets URI.</param>
        /// <param name="times">The times.</param>
        /// <param name="name">The name.</param>
        /// <param name="expectedContentType">Expected type of the content.</param>
        /// <param name="expectedRequestContent">Expected content of the request.</param>
        /// <param name="expectedRequestHeaders">The expected request headers.</param>
        /// <param name="requestValidator">The request validator.</param>
        /// <returns></returns>
        public static IRequestExpectation SetUpExpectation<T>(
            this HttpServerMock serverMock,
            Method method,
            string requetsUri,
            uint times = 1,
            string name = "",
            string expectedContentType = "None",
            T expectedRequestContent = null,
            IDictionary<string, string> expectedRequestHeaders = null,
            Func<HttpRequestMessage, bool> requestValidator = null) where T : class
        {
            return CreateHttpServerExpectation(
                serverMock,
               method,
                requetsUri,
                name,
                times,
                expectedContentType,
                expectedRequestContent,
                expectedRequestHeaders,
                requestValidator);
        }

        /// <summary>
        /// Sets up get expectation.
        /// </summary>
        /// <param name="serverMock">The server.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="requetsUri">The requets URI.</param>
        /// <param name="name">The name.</param>
        /// <param name="times">The times.</param>
        /// <returns></returns>
        public static IRequestExpectation SetUpExpectation(
            this HttpServerMock serverMock,
            Method method,
            string requetsUri,
            string name = "",
            uint times = 1)
        {
            return CreateHttpServerExpectation(
                serverMock,
                method,
                requetsUri,
                name,
                times,
                "None",
                null,
                null,
                null);
        }

        /// <summary>
        /// Sets up get expectation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serverMock">The server.</param>
        /// <param name="requetsUri">The requets URI.</param>
        /// <param name="times">The times.</param>
        /// <param name="name">The name.</param>
        /// <param name="expectedContentType">Expected type of the content.</param>
        /// <param name="expectedRequestContent">Expected content of the request.</param>
        /// <param name="expectedRequestHeaders">The expected request headers.</param>
        /// <param name="requestValidator">The request validator.</param>
        /// <returns></returns>
        public static IRequestExpectation SetUpGetExpectation<T>(
            this HttpServerMock serverMock,
            string requetsUri,
            uint times = 1,
            string name = "",
            string expectedContentType = "None",
            T expectedRequestContent = null,
            IDictionary<string, string> expectedRequestHeaders = null,
            Func<HttpRequestMessage, bool> requestValidator = null) where T : class
        {
            return CreateHttpServerExpectation(
                serverMock,
                Method.GET,
                requetsUri,
                name,
                times,
                expectedContentType,
                expectedRequestContent,
                expectedRequestHeaders,
                requestValidator);
        }

        /// <summary>
        /// Sets up get expectation.
        /// </summary>
        /// <param name="serverMock">The server.</param>
        /// <param name="requetsUri">The requets URI.</param>
        /// <param name="name">The name.</param>
        /// <param name="times">The times.</param>
        /// <returns></returns>
        public static RequestExpectation SetUpGetExpectation(
            this HttpServerMock serverMock,
            string requetsUri,
            string name = "",
            uint times = 1)
        {
            return CreateHttpServerExpectation(
                serverMock,
                Method.GET,
                requetsUri,
                name,
                times,
                RequestContentType.None,
                null,
                null,
                null);
        }

        /// <summary>
        /// Sets up post expectation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serverMock">The server.</param>
        /// <param name="requetsUri">The requets URI.</param>
        /// <param name="times">The times.</param>
        /// <param name="name">The name.</param>
        /// <param name="expectedContentType">Expected type of the content.</param>
        /// <param name="expectedRequestContent">Expected content of the request.</param>
        /// <param name="expectedRequestHeaders">The expected request headers.</param>
        /// <param name="requestValidator">The request validator.</param>
        /// <returns></returns>
        public static RequestExpectation SetUpPostExpectation<T>(
            this HttpServerMock serverMock,
            string requetsUri,
            uint times = 1,
            string name = "",
            RequestContentType expectedContentType = RequestContentType.None,
            T expectedRequestContent = null,
            IDictionary<string, string> expectedRequestHeaders = null,
            Func<HttpRequestMessage, bool> requestValidator = null) where T : class
        {
            return CreateHttpServerExpectation(
                serverMock,
                Method.POST,
                requetsUri,
                name,
                times,
                expectedContentType,
                expectedRequestContent,
                expectedRequestHeaders,
                requestValidator);
        }

        /// <summary>
        /// Sets up post expectation.
        /// </summary>
        /// <param name="serverMock">The server.</param>
        /// <param name="requetsUri">The requets URI.</param>
        /// <param name="name">The name.</param>
        /// <param name="times">The times.</param>
        /// <returns></returns>
        public static RequestExpectation SetUpPostExpectation(
            this HttpServerMock serverMock,
            string requetsUri,
            string name = "",
            uint times = 1)
        {
            return CreateHttpServerExpectation(
                serverMock,
                Method.POST,
                requetsUri,
                name,
                times,
                RequestContentType.None,
                null,
                null,
                null);
        }

        /// <summary>
        /// Verifies all request expectations.
        /// </summary>
        /// <param name="serverMock">The server.</param>
        /// <exception cref="HttpServerCallsVerificationException"></exception>
        public static void VerifyAllRequestExpectations(this HttpServerMock serverMock)
        {
            const string ErrorMessageFormat = "Request Name:'{0}' | Request Uri: '{1}' | Request Method: '{2}' | Expected Request Headers: '{3}' | Expected Request Content: '{4}' | Expected Request Content Type: '{5}' | Expected Number Of Calls: '{6}' | Actual Number Of Calls: '{7}'";

            var uncompletedExpectations = serverMock.ServerRequestsState.RequestExpectations.Where(requestExpectation => requestExpectation.Repeats != requestExpectation.NumberOfCallsPerformed).ToList();

            if (uncompletedExpectations.Any())
            {
                var errorMessageList = uncompletedExpectations.Select(expect =>
                    {
                        string serializedHeaders = string.Join("|", expect.ExpectedRequestHeaders.Select(header => string.Concat(header.Key, " - ", header.Value)));
                        string requestContent = JsonConvert.SerializeObject(expect.ExpectedRequestContent);

                        return new
                        {
                            Name = expect.Name,
                            Uri = expect.RequestUri.AbsoluteUri,
                            Headers = serializedHeaders,
                            Method = expect.RequestHttpMethod.ToString(),
                            Content = requestContent,
                            ContentType = expect.ExpectedRequestContentType.ToString(),
                            ExpectedNumberOfCalls = expect.Repeats,
                            ActualNumberOfCalls = expect.NumberOfCallsPerformed
                        };
                    }).Select(
                    formattedExpect =>
                        string.Format(
                        CultureInfo.InvariantCulture,
                        ErrorMessageFormat,
                        formattedExpect.Name,
                        formattedExpect.Uri,
                        formattedExpect.Method,
                        formattedExpect.Headers,
                        formattedExpect.Content,
                        formattedExpect.ContentType,
                        formattedExpect.ExpectedNumberOfCalls,
                        formattedExpect.ActualNumberOfCalls));

                string errorMessage = string.Concat(
                    "Some requests expectations were not met: \r\n",
                    string.Join("\r\n", errorMessageList));

                throw new HttpServerCallsVerificationException(errorMessage);
            }
        }

        /// <summary>
        /// Verifies all request expectations and unexpected requests.
        /// </summary>
        /// <param name="serverMock">The server.</param>
        /// <exception cref="HttpServerCallsVerificationException"></exception>
        public static void VerifyAllRequestExpectationsAndUnexpectedRequests(this HttpServerMock serverMock)
        {
            const string ErrorMessageFormat = "Request Uri: '{0}' | Request Method: '{1}' | Request Headers: '{2}' | Request Content: '{3}'";

            VerifyAllRequestExpectations(serverMock);

            if (serverMock.ServerRequestsState.UnexpectedRequests.Count > 0)
            {
                var errorMessageList = serverMock.ServerRequestsState.UnexpectedRequests.Select(
                    unexpected =>
                    {
                        string serializedHeaders = string.Join(
                            "|",
                            unexpected.Headers.Select(
                                header => string.Concat(header.Key, " - ", string.Join(", ", header.Value))));

                        return string.Format(
                            CultureInfo.InvariantCulture,
                            ErrorMessageFormat,
                            unexpected.RequestUri.AbsoluteUri,
                            unexpected.Method.ToString(),
                            serializedHeaders,
                            unexpected.Content);
                    });

                string errorMessage = string.Concat(
                    "There were some unexpected requests: \r\n",
                    string.Join("\r\n", errorMessageList));

                throw new HttpServerCallsVerificationException(errorMessage);
            }
        }

        #region Private Methods
        private static IRequestExpectation CreateHttpServerExpectation(HttpServerMock serverMock,
            Method requestMethod,
            string requetsUri,
            string name,
            int times,
            string expectedContentType,
            IContent expectedRequestContent,
            IDictionary<string, string> expectedRequestHeaders,
            Func<Request, bool> requestValidator)
        {
            var expectation = new RequestExpectationBuilder()
                .WithName(name)
                .WithUri(requetsUri)
                .WithMethod(requestMethod)
                .WithNumberOfCalls(times)
                .WithContentType(expectedContentType)
                .WithContent(expectedRequestContent)
                .WithValidator(requestValidator)
                .WithHeaders(expectedRequestHeaders)
                .Build();

            serverMock.ServerRequestsState.AddExpectaction(expectation);

            return expectation;
        }
        #endregion
    }
}