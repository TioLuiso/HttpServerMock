﻿using HttpServerMock.Common;
using HttpServerMock.Common.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HttpServerMockTests
{
    using HMockTests.MockClasses;

    using HttpServerMock;
    using HttpServerMock.Exceptions;
    using HttpServerMock.ExtensionMethods;

    using RestSharp;
    using System.Collections.Generic;
    using System.Net;
    using Method = Method;

    [TestClass]
    public class HttpServerMockTest
    {
        private const int TestServerPort = 50000;

        private string serverBaseUrl;

        [TestInitialize]
        public void TestInitialize()
        {
            this.serverBaseUrl = string.Concat("http://", Dns.GetHostName(), ":50000");
        }

        #region Single Request Ok

        [TestMethod]
        [TestCategory("ThreadNotSafe")]
        public void HHttpServer_DeleteRequest_Ok()
        {
            using (var hserver = new HttpServerMock(TestServerPort))
            {
                hserver.SetupExpectation(new RequestExpectationBuilder()
                    .WithMethod(Method.DELETE)
                    .WithUri("http://localhost:50000/user/23")
                    .WithHeader("test", "test1")
                    .WithResponseBuilder(new ExpectationResponseBuilder()
                        .WithStatusCode(StatusCode.OK)
                        .WithContentType(ContentTypes.Json)
                        .WithJsonContent(new ResponseTestClass { Name = "response", IsOld = true, Age = 12 }))
                    .Build());

                var restClient = new RestClient(this.serverBaseUrl);
                var request = new RestRequest("/user/23");
                request.AddJsonBody(new { Name = "test", Id = 23 });
                request.AddHeader("test", "test1");
                request.Method = RestSharp.Method.DELETE;

                var response = restClient.Execute<ResponseTestClass>(request);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");
                Assert.IsNotNull(response.Data, "The response is empty.");
                Assert.IsNull(response.ErrorException, "The request contains an exception.");
                Assert.AreEqual("response", response.Data.Name, "The response data is not the expected.");
                Assert.IsTrue(response.Data.IsOld, "The response data is not the expected.");
                Assert.AreEqual(12, response.Data.Age, "The response data is not the expected.");

                hserver.VerifyAllRequestExpectationsAndUnexpectedRequests();
            }
        }

        [TestMethod]
        [TestCategory("ThreadNotSafe")]
        public void HHttpServer_GetRequest_Ok()
        {
            using (var hserver = new HttpServerMock(TestServerPort))
            {
                hserver.SetupExpectation(new RequestExpectationBuilder()
                    .WithMethod(Method.GET)
                    .WithUri("http://localhost:50000/user/23")
                    .WithHeader("test", "test1")
                    .WithResponseBuilder(new ExpectationResponseBuilder()
                        .WithStatusCode(StatusCode.OK)
                        .WithContentType(ContentTypes.Json)
                        .WithJsonContent(new ResponseTestClass { Name = "response", IsOld = true, Age = 12 }))
                    .Build());

                var restClient = new RestClient(this.serverBaseUrl);
                var request = new RestRequest("/user/23");
                request.AddJsonBody(new { Name = "test", Id = 23 });
                request.AddHeader("test", "test1");
                request.Method = RestSharp.Method.GET;

                var response = restClient.Execute<ResponseTestClass>(request);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");
                Assert.IsNotNull(response.Data, "The response is empty.");
                Assert.IsNull(response.ErrorException, "The request contains an exception.");
                Assert.AreEqual("response", response.Data.Name, "The response data is not the expected.");
                Assert.IsTrue(response.Data.IsOld, "The response data is not the expected.");
                Assert.AreEqual(12, response.Data.Age, "The response data is not the expected.");

                hserver.VerifyAllRequestExpectationsAndUnexpectedRequests();
            }
        }

        [TestMethod]
        [TestCategory("ThreadNotSafe")]
        public void HHttpServer_PostRequest_Ok()
        {
            using (var hserver = new HttpServerMock(TestServerPort))
            {
                hserver.SetupExpectation(new RequestExpectationBuilder()
                    .WithMethod(Method.POST)
                    .WithUri("http://localhost:50000/user/23")
                    .WithJsonContent(new { Name = "test", Id = 23 })
                    .WithContentType(ContentTypes.Json)
                    .WithHeader("test", "test1")
                    .WithResponseBuilder(new ExpectationResponseBuilder()
                        .WithStatusCode(StatusCode.OK)
                        .WithContentType(ContentTypes.Json)
                        .WithJsonContent(new ResponseTestClass { Name = "response", IsOld = true, Age = 12 }))
                    .Build());

                var restClient = new RestClient(this.serverBaseUrl);
                var request = new RestRequest("/user/23");
                request.AddJsonBody(new { Name = "test", Id = 23 });
                request.AddHeader("test", "test1");
                request.Method = RestSharp.Method.POST;

                var response = restClient.Execute<ResponseTestClass>(request);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");
                Assert.IsNotNull(response.Data, "The response is empty.");
                Assert.IsNull(response.ErrorException, "The request contains an exception.");
                Assert.AreEqual("response", response.Data.Name, "The response data is not the expected.");
                Assert.IsTrue(response.Data.IsOld, "The response data is not the expected.");
                Assert.AreEqual(12, response.Data.Age, "The response data is not the expected.");

                hserver.VerifyAllRequestExpectationsAndUnexpectedRequests();
            }
        }

        #endregion

        #region Default Status Code
        [TestMethod]
        [TestCategory("ThreadNotSafe")]
        public void HHttpServer_ServerDefaultRespondStatusCode_Ok()
        {
            using (var hserver = new HttpServerMock(TestServerPort))
            {
                hserver.ServerRequestsState.DefaultRespondStatusCode = StatusCode.NotModified;

                var restClient = new RestClient(this.serverBaseUrl);
                var request = new RestRequest("/user/23");
                request.Method = RestSharp.Method.GET;

                var response = restClient.Execute<ResponseTestClass>(request);
                Assert.AreEqual(HttpStatusCode.NotModified, response.StatusCode, "The respond status code is not the expected.");
            }
        }
        #endregion

        #region Request Validator
        [TestMethod]
        [TestCategory("ThreadNotSafe")]
        public void HHttpServer_RequestValidatorValidatesRequest_Ok()
        {
            using (var hserver = new HttpServerMock(TestServerPort))
            {
                hserver.SetupExpectation(new RequestExpectationBuilder()
                    .WithMethod(Method.GET)
                    .WithUri("http://localhost:50000/user/23")
                    .WithHeader("test", "test1")
                    .WithValidator(
                        req =>
                        {
                            return req.Uri.PathAndQuery == "/user/23";
                        })
                    .WithResponseBuilder(new ExpectationResponseBuilder()
                        .WithStatusCode(StatusCode.OK)
                        .WithContentType(ContentTypes.Json)
                        .WithJsonContent(new ResponseTestClass { Name = "response", IsOld = true, Age = 12 }))
                    .Build());

                var restClient = new RestClient(this.serverBaseUrl);
                var request = new RestRequest("/user/23");
                request.AddJsonBody(new { Name = "test", Id = 23 });
                request.AddHeader("test", "test1");
                request.Method = RestSharp.Method.GET;

                var response = restClient.Execute<ResponseTestClass>(request);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");
                Assert.IsNotNull(response.Data, "The response is empty.");
                Assert.IsNull(response.ErrorException, "The request contains an exception.");
                Assert.AreEqual("response", response.Data.Name, "The response data is not the expected.");
                Assert.IsTrue(response.Data.IsOld, "The response data is not the expected.");
                Assert.AreEqual(12, response.Data.Age, "The response data is not the expected.");

                hserver.VerifyAllRequestExpectationsAndUnexpectedRequests();
            }
        }

        [TestMethod]
        [TestCategory("ThreadNotSafe")]
        [ExpectedException(typeof(HttpServerCallsVerificationException))]
        public void HHttpServer_RequestValidatorRefuseRequest_HttpServerCallsVerificationException()
        {
            using (var hserver = new HttpServerMock(TestServerPort))
            {
                hserver.SetupExpectation(new RequestExpectationBuilder()
                    .WithMethod(Method.GET)
                    .WithUri("http://localhost:50000/user/23")
                    .WithHeader("test", "test1")
                    .WithValidator(req => false)
                    .Build());

                var restClient = new RestClient(this.serverBaseUrl);
                var request = new RestRequest("/user/23");
                request.AddJsonBody(new { Name = "test", Id = 23 });
                request.AddHeader("test", "test1");
                request.Method = RestSharp.Method.GET;

                var response = restClient.Execute<ResponseTestClass>(request);
                Assert.AreEqual(HttpStatusCode.NotImplemented, response.StatusCode, "The respond status code is not the expected.");
                Assert.IsNull(response.Data, "The response is not empty.");
                Assert.IsNull(response.ErrorException, "The request contains an exception.");

                hserver.VerifyAllRequestExpectationsAndUnexpectedRequests();
            }
        }
        #endregion

        #region Request Repeats
        [TestMethod]
        [TestCategory("ThreadNotSafe")]
        public void HHttpServer_RepeatedRequests_Ok()
        {
            using (var hserver = new HttpServerMock(TestServerPort))
            {
                hserver.SetupExpectation(new RequestExpectationBuilder()
                    .WithMethod(Method.POST)
                    .WithUri("http://localhost:50000/user/23")
                    .WithNumberOfCalls(2)
                    .WithResponseBuilder(new ExpectationResponseBuilder()
                        .WithStatusCode(StatusCode.OK))
                    .Build());

                var restClient = new RestClient(this.serverBaseUrl);
                var request = new RestRequest("/user/23") { Method = RestSharp.Method.POST };

                var response = restClient.Execute(request);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");

                response = restClient.Execute(request);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");

                hserver.VerifyAllRequestExpectationsAndUnexpectedRequests();
            }
        }

        [TestMethod]
        [TestCategory("ThreadNotSafe")]
        [ExpectedException(typeof(HttpServerCallsVerificationException))]
        public void HHttpServer_RepeatedRequestsOneMore_NotImplementedRespondHttpStatus()
        {
            using (var hserver = new HttpServerMock(TestServerPort))
            {
                hserver.SetupExpectation(new RequestExpectationBuilder()
                    .WithMethod(Method.POST)
                    .WithUri("http://localhost:50000/user/23")
                    .WithNumberOfCalls(2)
                    .WithResponseBuilder(new ExpectationResponseBuilder()
                        .WithStatusCode(StatusCode.OK))
                    .Build());

                var restClient = new RestClient(this.serverBaseUrl);
                var request = new RestRequest("/user/23") { Method = RestSharp.Method.POST };

                var response = restClient.Execute(request);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");

                response = restClient.Execute(request);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");

                response = restClient.Execute(request);
                Assert.AreEqual(HttpStatusCode.NotImplemented, response.StatusCode, "The respond status code is not the expected.");

                hserver.VerifyAllRequestExpectationsAndUnexpectedRequests();
            }
        }
        #endregion

        #region Request Headers Validation

        [TestMethod]
        [TestCategory("ThreadNotSafe")]
        public void HHttpServer_CustomHeadersExpected_Ok()
        {
            using (var hserver = new HttpServerMock(TestServerPort))
            {
                hserver.SetupExpectation(new RequestExpectationBuilder()
                    .WithMethod(Method.POST)
                    .WithUri("http://localhost:50000/user/23")
                    .WithHeaders(new Dictionary<string, IEnumerable<string>>{{"test", new []{ "value", "value2" }}, {"test2", new[]{"value3"}}})
                    .WithResponseBuilder(new ExpectationResponseBuilder()
                        .WithStatusCode(StatusCode.OK))
                    .Build());

                var restClient = new RestClient(this.serverBaseUrl);
                var request = new RestRequest("/user/23");
                request.AddHeader("test", "value");
                request.AddHeader("test", "value2");
                request.AddHeader("test2", "value3");
                request.AddHeader("test3", "value4");
                request.Method = RestSharp.Method.POST;

                var response = restClient.Execute(request);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");
            }
        }

        [TestMethod]
        [TestCategory("ThreadNotSafe")]
        public void HHttpServer_CustomHeaderExpected_Ok()
        {
            using (var hserver = new HttpServerMock(TestServerPort))
            {
                hserver.SetupExpectation(new RequestExpectationBuilder()
                    .WithMethod(Method.POST)
                    .WithUri("http://localhost:50000/user/23")
                    .WithHeader("test", "value")
                    .WithResponseBuilder(new ExpectationResponseBuilder()
                        .WithStatusCode(StatusCode.OK))
                    .Build());

                var restClient = new RestClient(this.serverBaseUrl);
                var request = new RestRequest("/user/23");
                request.AddHeader("test", "value");
                request.Method = RestSharp.Method.POST;

                var response = restClient.Execute(request);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");
            }
        }

        [TestMethod]
        [TestCategory("ThreadNotSafe")]
        public void HHttpServer_RequestContentSpecialHeaderExpected_Ok()
        {
            using (var hserver = new HttpServerMock(TestServerPort))
            {
                hserver.SetupExpectation(new RequestExpectationBuilder()
                    .WithMethod(Method.POST)
                    .WithUri("http://localhost:50000/user/23")
                    .WithHeader("Content-Type", "application/text")
                    .WithResponseBuilder(new ExpectationResponseBuilder()
                        .WithStatusCode(StatusCode.OK))
                    .Build());

                var restClient = new RestClient(this.serverBaseUrl);
                var request = new RestRequest("/user/23");
                request.AddHeader("Content-Type", "application/text");
                request.Method = RestSharp.Method.POST;

                var response = restClient.Execute(request);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");

                hserver.VerifyAllRequestExpectationsAndUnexpectedRequests();
            }
        }

        [TestMethod]
        [TestCategory("ThreadNotSafe")]
        public void HHttpServer_RequestSpecialHeaderExpected_Ok()
        {
            using (var hserver = new HttpServerMock(TestServerPort))
            {
                hserver.SetupExpectation(new RequestExpectationBuilder()
                    .WithMethod(Method.POST)
                    .WithUri("http://localhost:50000/user/23")
                    .WithHeader("Host", Dns.GetHostName().ToLower() + ":50000")
                    .WithResponseBuilder(new ExpectationResponseBuilder()
                        .WithStatusCode(StatusCode.OK))
                    .Build());

                var restClient = new RestClient(this.serverBaseUrl);
                var request = new RestRequest("/user/23");
                request.Method = RestSharp.Method.POST;

                var response = restClient.Execute(request);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");
            }
        }

        [TestMethod]
        [TestCategory("ThreadNotSafe")]
        public void HHttpServer_ExpectedHeaderNotPresent_NotImplementedRespondHttpStatus()
        {
            using (var hserver = new HttpServerMock(TestServerPort))
            {
                hserver.SetupExpectation(new RequestExpectationBuilder()
                    .WithMethod(Method.POST)
                    .WithUri("http://localhost:50000/user/23")
                    .WithHeader("expectedHeader", "headerValue")
                    .WithResponseBuilder(new ExpectationResponseBuilder()
                        .WithStatusCode(StatusCode.OK))
                    .Build());

                var restClient = new RestClient(this.serverBaseUrl);
                var request = new RestRequest("/user/23");
                request.AddHeader("test", "test1");
                request.Method = RestSharp.Method.POST;

                var response = restClient.Execute<ResponseTestClass>(request);
                Assert.AreEqual(HttpStatusCode.NotImplemented, response.StatusCode, "The respond status code is not the expected.");
                Assert.IsNull(response.Data, "The response is not empty.");
                Assert.IsNull(response.ErrorException, "The request contains an exception.");
            }
        }

        [TestMethod]
        [TestCategory("ThreadNotSafe")]
        public void HHttpServer_ExpectedHeaderPresentDifferentValue__NotImplementedRespondHttpStatus()
        {
            using (var hserver = new HttpServerMock(TestServerPort))
            {
                hserver.SetupExpectation(new RequestExpectationBuilder()
                    .WithMethod(Method.POST)
                    .WithUri("http://localhost:50000/user/23")
                    .WithHeader("expectedHeader", "headerValue")
                    .WithResponseBuilder(new ExpectationResponseBuilder()
                        .WithStatusCode(StatusCode.OK))
                    .Build());

                var restClient = new RestClient(this.serverBaseUrl);
                var request = new RestRequest("/user/23");
                request.AddHeader("expectedHeader", "test");
                request.Method = RestSharp.Method.POST;

                var response = restClient.Execute<ResponseTestClass>(request);
                Assert.AreEqual(HttpStatusCode.NotImplemented, response.StatusCode, "The respond status code is not the expected.");
                Assert.IsNull(response.Data, "The response is not empty.");
                Assert.IsNull(response.ErrorException, "The request contains an exception.");
            }
        }

        [TestMethod]
        [TestCategory("ThreadNotSafe")]
        public void HHttpServer_ExpectedHeaderPresentDifferentValueCase_NotImplementedRespondHttpStatus()
        {
            using (var hserver = new HttpServerMock(TestServerPort))
            {
                hserver.SetupExpectation(new RequestExpectationBuilder()
                    .WithMethod(Method.POST)
                    .WithUri("http://localhost:50000/user/23")
                    .WithHeader("expectedHeader", "headerValue")
                    .WithResponseBuilder(new ExpectationResponseBuilder()
                        .WithStatusCode(StatusCode.OK))
                    .Build());

                var restClient = new RestClient(this.serverBaseUrl);
                var request = new RestRequest("/user/23");
                request.AddHeader("expectedHeader", "headervalue");
                request.Method = RestSharp.Method.POST;

                var response = restClient.Execute<ResponseTestClass>(request);
                Assert.AreEqual(HttpStatusCode.NotImplemented, response.StatusCode, "The respond status code is not the expected.");
                Assert.IsNull(response.Data, "The response is not empty.");
                Assert.IsNull(response.ErrorException, "The request contains an exception.");
            }
        }

        [TestMethod]
        [TestCategory("ThreadNotSafe")]
        public void HHttpServer_ExpectedHeaderPresentDifferentNameCase_Ok()
        {
            using (var hserver = new HttpServerMock(TestServerPort))
            {
                hserver.SetupExpectation(new RequestExpectationBuilder()
                    .WithMethod(Method.POST)
                    .WithUri("http://localhost:50000/user/23")
                    .WithHeader("expectedHeader", "HeaderValue")
                    .WithResponseBuilder(new ExpectationResponseBuilder()
                        .WithStatusCode(StatusCode.OK))
                    .Build());

                var restClient = new RestClient(this.serverBaseUrl);
                var request = new RestRequest("/user/23");
                request.AddHeader("expectedheader", "HeaderValue");
                request.Method = RestSharp.Method.POST;

                var response = restClient.Execute<ResponseTestClass>(request);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");

                hserver.VerifyAllRequestExpectationsAndUnexpectedRequests();
            }
        }
        #endregion

        #region Request Content Validation
        [TestMethod]
        [TestCategory("ThreadNotSafe")]
        public void HHttpServer_JsonRequestContentMorePropertiesThanExpected_NotImplementedRespondHttpStatus()
        {
            using (var hserver = new HttpServerMock(TestServerPort))
            {
                hserver.SetupExpectation(new RequestExpectationBuilder()
                    .WithMethod(Method.POST)
                    .WithUri("http://localhost:50000/user/23")
                    .WithContentType(ContentTypes.Json)
                    .WithJsonContent(new { Name = "test", Id = 23 })
                    .WithResponseBuilder(new ExpectationResponseBuilder()
                        .WithStatusCode(StatusCode.OK))
                    .Build());

                var restClient = new RestClient(this.serverBaseUrl);
                var request = new RestRequest("/user/23");
                request.AddJsonBody(new { Name = "test", Id = 23, IsOld = true });
                request.Method = RestSharp.Method.POST;

                var response = restClient.Execute(request);
                Assert.AreEqual(HttpStatusCode.NotImplemented, response.StatusCode, "The respond status code is not the expected.");
                Assert.IsNull(response.ErrorException, "The request contains an exception.");
            }
        }

        [TestMethod]
        [TestCategory("ThreadNotSafe")]
        public void HHttpServer_JsonRequestContentDifferentLetterCase_NotImplementedRespondHttpStatus()
        {
            using (var hserver = new HttpServerMock(TestServerPort))
            {
                hserver.SetupExpectation(new RequestExpectationBuilder()
                    .WithMethod(Method.POST)
                    .WithUri("http://localhost:50000/user/23")
                    .WithContentType(ContentTypes.Json)
                    .WithJsonContent(new { Name = "TesT", Id = 23, IsOld = true })
                    .WithResponseBuilder(new ExpectationResponseBuilder()
                        .WithStatusCode(StatusCode.OK))
                    .Build());

                var restClient = new RestClient(this.serverBaseUrl);
                var request = new RestRequest("/user/23");
                request.AddJsonBody(new { Name = "test", Id = 23, IsOld = true });
                request.Method = RestSharp.Method.POST;

                var response = restClient.Execute(request);
                Assert.AreEqual(HttpStatusCode.NotImplemented, response.StatusCode, "The respond status code is not the expected.");
                Assert.IsNull(response.ErrorException, "The request contains an exception.");
            }
        }

        [TestMethod]
        [TestCategory("ThreadNotSafe")]
        public void HHttpServer_JsonValidRequestContent_Ok()
        {
            using (var hserver = new HttpServerMock(TestServerPort))
            {
                hserver.SetupExpectation(new RequestExpectationBuilder()
                    .WithMethod(Method.POST)
                    .WithUri("http://localhost:50000/user/23")
                    .WithContentType(ContentTypes.Json)
                    .WithJsonContent(new { Name = "test", IsOld = true, Id = 23 })
                    .WithResponseBuilder(new ExpectationResponseBuilder()
                        .WithStatusCode(StatusCode.OK))
                    .Build());

                var restClient = new RestClient(this.serverBaseUrl);
                var request = new RestRequest("/user/23");
                request.AddJsonBody(new { Name = "test", Id = 23, IsOld = true });
                request.Method = RestSharp.Method.POST;

                var response = restClient.Execute(request);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");
                Assert.IsNull(response.ErrorException, "The request contains an exception.");
            }
        }

        [TestMethod]
        [TestCategory("ThreadNotSafe")]
        public void HHttpServer_JsonStringContent_Ok()
        {
            using (var hserver = new HttpServerMock(TestServerPort))
            {
                hserver.SetupExpectation(new RequestExpectationBuilder()
                    .WithMethod(Method.POST)
                    .WithUri("http://localhost:50000/user/23")
                    .WithContentType(ContentTypes.Json)
                    .WithJsonContent("{\"Name\":\"test\", \"IsOld\":true, \"Id\":23}")
                    .WithResponseBuilder(new ExpectationResponseBuilder()
                        .WithStatusCode(StatusCode.OK))
                    .Build());

                var restClient = new RestClient(this.serverBaseUrl);
                var request = new RestRequest("/user/23");
                request.AddJsonBody(new { Name = "test", Id = 23, IsOld = true });
                request.Method = RestSharp.Method.POST;

                var response = restClient.Execute(request);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");
                Assert.IsNull(response.ErrorException, "The request contains an exception.");
            }
        }

        [TestMethod]
        [TestCategory("ThreadNotSafe")]
        public void HHttpServer_XmlStringContent_Ok()
        {
            using (var hserver = new HttpServerMock(TestServerPort))
            {
                hserver.SetupExpectation(new RequestExpectationBuilder()
                    .WithMethod(Method.POST)
                    .WithUri("http://localhost:50000/user/23")
                    .WithContentType(ContentTypes.Xml)
                    .WithXmlContent("<ResponseTestClass><Name>test</Name><IsOld>true</IsOld><Age>23</Age></ResponseTestClass>")
                    .WithResponseBuilder(new ExpectationResponseBuilder()
                        .WithStatusCode(StatusCode.OK)
                        .WithContentType(ContentTypes.Xml)
                        .WithXmlContent(new { Name = "testres", Age = 25, IsOld = false }))
                    .Build());

                var restClient = new RestClient(this.serverBaseUrl);
                var request = new RestRequest("/user/23");
                request.AddXmlBody(new ResponseTestClass() { Name = "test", Age = 23, IsOld = true });
                request.Method = RestSharp.Method.POST;

                var response = restClient.Execute<ResponseTestClass>(request);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");
                Assert.IsNull(response.ErrorException, "The request contains an exception.");
                Assert.AreEqual("testres", response.Data.Name, "The data returned by the server is not the expected.");
                Assert.AreEqual(false, response.Data.IsOld, "The data returned by the server is not the expected.");
                Assert.AreEqual(25, response.Data.Age, "The data returned by the server is not the expected.");
            }
        }

        [TestMethod]
        [TestCategory("ThreadNotSafe")]
        public void HHttpServer_XmlContentDynamic_Ok()
        {
            using (var hserver = new HttpServerMock(TestServerPort))
            {
                hserver.SetupExpectation(new RequestExpectationBuilder()
                    .WithMethod(Method.POST)
                    .WithUri("http://localhost:50000/user/23")
                    .WithContentType(ContentTypes.Xml)
                    .WithXmlContent(new { Name = "test", IsOld = true, Age = 23 })
                    .WithResponseBuilder(new ExpectationResponseBuilder()
                        .WithStatusCode(StatusCode.OK)
                        .WithContentType("application/xml;charset=utf-8")
                        .WithXmlContent(new { Name = "testres", Age = 25, IsOld = false }))
                    .Build());

                var restClient = new RestClient(this.serverBaseUrl);
                var request = new RestRequest("/user/23");
                request.AddXmlBody(new ResponseTestClass() { Name = "test", Age = 23, IsOld = true });
                request.Method = RestSharp.Method.POST;

                var response = restClient.Execute<ResponseTestClass>(request);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");
                Assert.IsNull(response.ErrorException, "The request contains an exception.");
                Assert.AreEqual("testres", response.Data.Name, "The data returned by the server is not the expected.");
                Assert.AreEqual(false, response.Data.IsOld, "The data returned by the server is not the expected.");
                Assert.AreEqual(25, response.Data.Age, "The data returned by the server is not the expected.");
            }
        }

        [TestMethod]
        [TestCategory("ThreadNotSafe")]
        public void HHttpServer_XmlContentTyped_Ok()
        {
            using (var hserver = new HttpServerMock(TestServerPort))
            {
                hserver.SetupExpectation(new RequestExpectationBuilder()
                    .WithMethod(Method.POST)
                    .WithUri("http://localhost:50000/user/23")
                    .WithContentType(ContentTypes.Xml)
                    .WithXmlContent(new ResponseTestClass() { Name = "test", IsOld = true, Age = 23 })
                    .WithResponseBuilder(new ExpectationResponseBuilder()
                        .WithStatusCode(StatusCode.OK)
                        .WithContentType("application/xml;charset=utf-8")
                        .WithXmlContent(new { Name = "testres", Age = 25, IsOld = false }))
                    .Build());

                var restClient = new RestClient(this.serverBaseUrl);
                var request = new RestRequest("/user/23");
                request.AddXmlBody(new ResponseTestClass() { Name = "test", Age = 23, IsOld = true });
                request.Method = RestSharp.Method.POST;

                var response = restClient.Execute<ResponseTestClass>(request);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");
                Assert.IsNull(response.ErrorException, "The request contains an exception.");
                Assert.AreEqual("testres", response.Data.Name, "The data returned by the server is not the expected.");
                Assert.AreEqual(false, response.Data.IsOld, "The data returned by the server is not the expected.");
                Assert.AreEqual(25, response.Data.Age, "The data returned by the server is not the expected.");
            }
        }

        //[TestMethod]
        //[TestCategory("ThreadNotSafe")]
        //public void HHttpServer_FormUrlEncodedContentTyped_Ok()
        //{
        //    using (var hserver = new HHttpServer(TestServerPort))
        //    {
        //        hserver.SetUpPostExpectation("user/23")
        //            .ExpectedContent(new ResponseTestClass() { Name = "test", IsOld = true, Age = 23 }, HContentType.FormUrlEncoded)
        //            .Response(HttpStatusCode.OK, HContentType.FormUrlEncoded, new { Name = "testres", Age = 25, IsOld = false });

        //        var restClient = new RestClient(this.serverBaseUrl);
        //        var request = new RestRequest("/user/23");
        //        request.AddObject(new ResponseTestClass() { Name = "test", Age = 23, IsOld = true });
        //        request.Method = Method.POST;

        //        var response = restClient.Execute<ResponseTestClass>(request);
        //        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");
        //        Assert.IsNull(response.ErrorException, "The request contains an exception.");
        //        Assert.AreEqual("testres", response.Data.Name, "The data returned by the server is not the expected.");
        //        Assert.AreEqual(false, response.Data.IsOld, "The data returned by the server is not the expected.");
        //        Assert.AreEqual(25, response.Data.Age, "The data returned by the server is not the expected.");
        //    }
        //}
        #endregion

        //#region Request Content Type Validation
        //[TestMethod]
        //[TestCategory("ThreadNotSafe")]
        //public void HHttpServer_NoneContentTypeIsNotChecked_Ok()
        //{
        //    using (var hserver = new HttpServerMock(TestServerPort))
        //    {
        //        hserver.SetUpPostExpectation("user/23")
        //            .ExpectedContent((object)null, RequestContentType.None)
        //            .Response(HttpStatusCode.OK);

        //        var restClient = new RestClient(this.serverBaseUrl);
        //        var request = new RestRequest("/user/23");
        //        request.AddHeader("Content-Type", "application/text");
        //        request.Method = RestSharp.Method.POST;

        //        var response = restClient.Execute(request);
        //        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");
        //        Assert.IsNull(response.ErrorException, "The request contains an exception.");
        //    }
        //}

        //[TestMethod]
        //[TestCategory("ThreadNotSafe")]
        //public void HHttpServer_NoneContentType_Ok()
        //{
        //    using (var hserver = new HttpServerMock(TestServerPort))
        //    {
        //        hserver.SetUpPostExpectation("user/23")
        //            .ExpectedContent((object)null, RequestContentType.Json)
        //            .Response(HttpStatusCode.OK);

        //        var restClient = new RestClient(this.serverBaseUrl);
        //        var request = new RestRequest("/user/23");
        //        request.AddHeader("Content-Type", "application/json");
        //        request.Method = RestSharp.Method.POST;

        //        var response = restClient.Execute(request);
        //        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");
        //        Assert.IsNull(response.ErrorException, "The request contains an exception.");
        //    }
        //}

        //[TestMethod]
        //[TestCategory("ThreadNotSafe")]
        //public void HHttpServer_UnexpectedContentType_NotImplementedRespondHttpStatus()
        //{
        //    using (var hserver = new HttpServerMock(TestServerPort))
        //    {
        //        hserver.SetUpPostExpectation("user/23")
        //            .ExpectedContent((object)null, RequestContentType.Json)
        //            .Response(HttpStatusCode.OK);

        //        var restClient = new RestClient(this.serverBaseUrl);
        //        var request = new RestRequest("/user/23");
        //        request.AddHeader("Content-Type", "application/text");
        //        request.Method = RestSharp.Method.POST;

        //        var response = restClient.Execute(request);
        //        Assert.AreEqual(HttpStatusCode.NotImplemented, response.StatusCode, "The respond status code is not the expected.");
        //        Assert.IsNull(response.ErrorException, "The request contains an exception.");
        //    }
        //}
        //#endregion

        //#region Request Method Validation
        //[TestMethod]
        //[TestCategory("ThreadNotSafe")]
        //public void HHttpServer_DifferentRequestMethod_NotImplementedRespondHttpStatus()
        //{
        //    using (var hserver = new HttpServerMock(TestServerPort))
        //    {
        //        hserver.SetUpGetExpectation("http://localhost:50000/user/23")
        //            .Response(HttpStatusCode.OK);

        //        var restClient = new RestClient(this.serverBaseUrl);
        //        var request = new RestRequest("/user/23") { Method = RestSharp.Method.POST };

        //        var response = restClient.Execute(request);
        //        Assert.AreEqual(HttpStatusCode.NotImplemented, response.StatusCode, "The respond status code is not the expected.");
        //        Assert.IsNull(response.ErrorException, "The request contains an exception.");
        //    }
        //}
        //#endregion

        //#region Request Uri Regex Validation
        //[TestMethod]
        //[TestCategory("ThreadNotSafe")]
        //public void HHttpServer_ValidUriRegex_Ok()
        //{
        //    using (var hserver = new HttpServerMock(TestServerPort))
        //    {
        //        hserver.SetUpGetExpectation("user/[0-9]{2}")
        //            .Response(HttpStatusCode.OK);

        //        var restClient = new RestClient(this.serverBaseUrl);
        //        var request = new RestRequest("/user/23/data") { Method = RestSharp.Method.GET };

        //        var response = restClient.Execute(request);
        //        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");
        //        Assert.IsNull(response.ErrorException, "The request contains an exception.");
        //    }
        //}

        //[TestMethod]
        //[TestCategory("ThreadNotSafe")]
        //public void HHttpServer_ValidStrictUriRegexFails__NotImplementedRespondHttpStatus()
        //{
        //    using (var hserver = new HttpServerMock(TestServerPort))
        //    {
        //        hserver.SetUpGetExpectation("^user/[0-9]{2}$")
        //            .Response(HttpStatusCode.OK);

        //        var restClient = new RestClient(this.serverBaseUrl);
        //        var request = new RestRequest("/user/23/data") { Method = RestSharp.Method.GET };

        //        var response = restClient.Execute(request);
        //        Assert.AreEqual(HttpStatusCode.NotImplemented, response.StatusCode, "The respond status code is not the expected.");
        //        Assert.IsNull(response.ErrorException, "The request contains an exception.");
        //    }
        //}

        //[TestMethod]
        //[TestCategory("ThreadNotSafe")]
        //public void HHttpServer_ValidStrictUriRegex_Ok()
        //{
        //    using (var hserver = new HttpServerMock(TestServerPort))
        //    {
        //        hserver.SetUpGetExpectation("^/user/[0-9]{2}/data\\?name=paco$")
        //            .Response(HttpStatusCode.OK);

        //        var restClient = new RestClient(this.serverBaseUrl);
        //        var request = new RestRequest("/user/23/data?name=paco") { Method = RestSharp.Method.GET };

        //        var response = restClient.Execute(request);
        //        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");
        //        Assert.IsNull(response.ErrorException, "The request contains an exception.");
        //    }
        //}
        //#endregion

        //#region Request Uri Validation
        //[TestMethod]
        //[TestCategory("ThreadNotSafe")]
        //public void HHttpServer_DifferentRequestUri_NotImplementedRespondHttpStatus()
        //{
        //    using (var hserver = new HttpServerMock(TestServerPort))
        //    {
        //        hserver.SetUpGetExpectation("http://localhost:50000/user/24")
        //            .Response(HttpStatusCode.OK);

        //        var restClient = new RestClient(this.serverBaseUrl);
        //        var request = new RestRequest("/user/23") { Method = RestSharp.Method.GET };

        //        var response = restClient.Execute(request);
        //        Assert.AreEqual(HttpStatusCode.NotImplemented, response.StatusCode, "The respond status code is not the expected.");
        //        Assert.IsNull(response.ErrorException, "The request contains an exception.");
        //    }
        //}

        //[TestMethod]
        //[TestCategory("ThreadNotSafe")]
        //public void HHttpServer_DifferentRequestUriLetterCase_Ok()
        //{
        //    using (var hserver = new HttpServerMock(TestServerPort))
        //    {
        //        hserver.SetUpGetExpectation("http://localhost:50000/usER/23")
        //            .Response(HttpStatusCode.OK);

        //        var restClient = new RestClient(this.serverBaseUrl);
        //        var request = new RestRequest("/user/23") { Method = RestSharp.Method.GET };

        //        var response = restClient.Execute(request);
        //        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");
        //        Assert.IsNull(response.ErrorException, "The request contains an exception.");
        //    }
        //}

        //[TestMethod]
        //[TestCategory("ThreadNotSafe")]
        //public void HHttpServer_RelativeUri_Ok()
        //{
        //    using (var hserver = new HttpServerMock(TestServerPort))
        //    {
        //        hserver.SetUpGetExpectation("user/23")
        //            .Response(HttpStatusCode.OK);

        //        var restClient = new RestClient(this.serverBaseUrl);
        //        var request = new RestRequest("/user/23") { Method = RestSharp.Method.GET };

        //        var response = restClient.Execute(request);
        //        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");
        //        Assert.IsNull(response.ErrorException, "The request contains an exception.");
        //    }
        //}

        //[TestMethod]
        //[TestCategory("ThreadNotSafe")]
        //public void HHttpServer_RequestMockUriUrlEncodedOriginalRequestUriUrlEncoded_Ok()
        //{
        //    using (var hserver = new HttpServerMock(TestServerPort))
        //    {
        //        hserver.SetUpGetExpectation("http://localhost:50000/user/age?q=%3E23")
        //            .Response(HttpStatusCode.OK);

        //        var restClient = new RestClient(this.serverBaseUrl);
        //        var request = new RestRequest("/user/age?q=%3E23") { Method = RestSharp.Method.GET };

        //        var response = restClient.Execute(request);
        //        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");
        //        Assert.IsNull(response.ErrorException, "The request contains an exception.");
        //    }
        //}

        //[TestMethod]
        //[TestCategory("ThreadNotSafe")]
        //public void HHttpServer_RequestMockUriUrlEncodedOriginalRequestUriNoUrlEncoded_Ok()
        //{
        //    using (var hserver = new HttpServerMock(TestServerPort))
        //    {
        //        hserver.SetUpGetExpectation("http://localhost:50000/user/age?q=%3E23")
        //            .Response(HttpStatusCode.OK);

        //        var restClient = new RestClient(this.serverBaseUrl);
        //        var request = new RestRequest("/user/age?q=>E23") { Method = RestSharp.Method.GET };

        //        var response = restClient.Execute(request);
        //        Assert.AreEqual(HttpStatusCode.NotImplemented, response.StatusCode, "The respond status code is not the expected.");
        //        Assert.IsNull(response.ErrorException, "The request contains an exception.");
        //    }
        //}
        //#endregion

        //#region Request Timeout
        //[TestMethod]
        //[TestCategory("ThreadNotSafe")]
        //public void HHttpServer_Timout_Ok()
        //{
        //    using (var hserver = new HttpServerMock(TestServerPort))
        //    {
        //        hserver.SetUpGetExpectation("http://localhost:50000/usER/23")
        //            .TimedOut();

        //        var restClient = new RestClient(this.serverBaseUrl);
        //        var request = new RestRequest("/user/23") { Method = RestSharp.Method.GET , Timeout = 100};

        //        var response = restClient.Execute(request);
        //        Assert.AreEqual(ResponseStatus.TimedOut, response.ResponseStatus, "The request does not timed out.");
        //    }
        //}
        //#endregion

        //#region Request Sequences
        //[TestMethod]
        //[TestCategory("ThreadNotSafe")]
        //public void HHttpServer_TwoSimilarRequestsTwoDifferentExpectations_Ok()
        //{
        //    using (var hserver = new HttpServerMock(TestServerPort))
        //    {
        //        hserver.SetUpPostExpectation("http://localhost:50000/user/23")
        //            .ExpectedNumberOfCalls(1)
        //            .ExpectedContent(new { Name = "test", Id = 23 }, RequestContentType.Json)
        //            .ExpectedRequestHeader("test", "test1")
        //            .Response(HttpStatusCode.OK, RequestContentType.Json, new ResponseTestClass { Name = "response", IsOld = true, Age = 12 });

        //        hserver.SetUpPostExpectation("http://localhost:50000/user/23")
        //            .ExpectedNumberOfCalls(1)
        //            .ExpectedContent(new { Name = "test", Id = 23 }, RequestContentType.Json)
        //            .ExpectedRequestHeader("test", "test1")
        //            .Response(HttpStatusCode.OK, RequestContentType.Json, new ResponseTestClass { Name = "response2", IsOld = false, Age = 13 });

        //        var restClient = new RestClient(this.serverBaseUrl);
        //        var request = new RestRequest("/user/23");
        //        request.AddJsonBody(new { Name = "test", Id = 23 });
        //        request.AddHeader("test", "test1");
        //        request.Method = RestSharp.Method.POST;

        //        var response = restClient.Execute<ResponseTestClass>(request);
        //        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");
        //        Assert.IsNotNull(response.Data, "The response is empty.");
        //        Assert.IsNull(response.ErrorException, "The request contains an exception.");
        //        Assert.AreEqual("response", response.Data.Name, "The response data is not the expected.");
        //        Assert.IsTrue(response.Data.IsOld, "The response data is not the expected.");
        //        Assert.AreEqual(12, response.Data.Age, "The response data is not the expected.");

        //        var response2 = restClient.Execute<ResponseTestClass>(request);
        //        Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode, "The respond status code is not the expected.");
        //        Assert.IsNotNull(response2.Data, "The response is empty.");
        //        Assert.IsNull(response2.ErrorException, "The request contains an exception.");
        //        Assert.AreEqual("response2", response2.Data.Name, "The response data is not the expected.");
        //        Assert.IsFalse(response2.Data.IsOld, "The response data is not the expected.");
        //        Assert.AreEqual(13, response2.Data.Age, "The response data is not the expected.");

        //        hserver.VerifyAllRequestExpectationsAndUnexpectedRequests();
        //    }
        //}
        //#endregion

        //#region Response Builder
        //[TestMethod]
        //[TestCategory("ThreadNotSafe")]
        //public void HHttpServer_ResponseBuilder_Ok()
        //{
        //    using (var hserver = new HttpServerMock(TestServerPort))
        //    {
        //        hserver.SetUpGetExpectation("http://localhost:50000/user/23").ExpectedNumberOfCalls(1).Response(
        //            req =>
        //            {
        //                var resp = new HttpResponseMessage
        //                {
        //                    Content =
        //                        new StringContent(
        //                        "{\"Name\":\"test\", \"IsOld\":false, \"Age\":1}")
        //                };

        //                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //                return resp;
        //            });

        //        var restClient = new RestClient(this.serverBaseUrl);
        //        var request = new RestRequest("/user/23");
        //        request.AddJsonBody(new { Name = "test", Id = 23 });
        //        request.AddHeader("test", "test1");
        //        request.Method = RestSharp.Method.GET;

        //        var response = restClient.Execute<ResponseTestClass>(request);
        //        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");
        //        Assert.IsNotNull(response.Data, "The response is empty.");
        //        Assert.IsNull(response.ErrorException, "The request contains an exception.");
        //        Assert.AreEqual("test", response.Data.Name, "The response data is not the expected.");
        //        Assert.IsFalse(response.Data.IsOld, "The response data is not the expected.");
        //        Assert.AreEqual(1, response.Data.Age, "The response data is not the expected.");

        //        hserver.VerifyAllRequestExpectationsAndUnexpectedRequests();
        //    }
        //}
        //#endregion

        //#region Response Add Headers
        //[TestMethod]
        //[TestCategory("ThreadNotSafe")]
        //public void HHttpServer_ResponseAddHeaders_Ok()
        //{
        //    using (var hserver = new HttpServerMock(TestServerPort))
        //    {
        //        hserver.SetUpGetExpectation("http://localhost:50000/user/23")
        //            .Response(HttpStatusCode.OK)
        //            .ResponseHeaders(new Dictionary<string, string>() { { "Test", "HeaderValue" } });

        //        var restClient = new RestClient(this.serverBaseUrl);
        //        var request = new RestRequest("/user/23");
        //        request.Method = RestSharp.Method.GET;

        //        var response = restClient.Execute(request);
        //        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");
        //        Assert.IsNull(response.ErrorException, "The request contains an exception.");
        //        Assert.IsTrue(response.Headers.Any(h => h.Name == "Test" && h.Value.ToString() == "HeaderValue"), "The response does not contains the header.");
        //    }
        //}

        //[TestMethod]
        //[TestCategory("ThreadNotSafe")]
        //public void HHttpServer_ResponseAddHeader_Ok()
        //{
        //    using (var hserver = new HttpServerMock(TestServerPort))
        //    {
        //        hserver.SetUpGetExpectation("http://localhost:50000/user/23")
        //            .Response(HttpStatusCode.OK)
        //            .ResponseHeader("Test", "HeaderValue");

        //        var restClient = new RestClient(this.serverBaseUrl);
        //        var request = new RestRequest("/user/23");
        //        request.Method = RestSharp.Method.GET;

        //        var response = restClient.Execute(request);
        //        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");
        //        Assert.IsNull(response.ErrorException, "The request contains an exception.");
        //        Assert.IsTrue(response.Headers.Any(h => h.Name == "Test" && h.Value.ToString() == "HeaderValue"), "The response does not contains the header.");
        //    }
        //}

        //[TestMethod]
        //[TestCategory("ThreadNotSafe")]
        //public void HHttpServer_ResponseContentSpecialHeader_Ok()
        //{
        //    using (var hserver = new HttpServerMock(TestServerPort))
        //    {
        //        hserver.SetUpGetExpectation("http://localhost:50000/user/23")
        //            .Response(HttpStatusCode.OK)
        //            .ResponseHeader("Content-Type", "application/text");

        //        var restClient = new RestClient(this.serverBaseUrl);
        //        var request = new RestRequest("/user/23");
        //        request.Method = RestSharp.Method.GET;

        //        var response = restClient.Execute(request);
        //        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");
        //        Assert.IsNull(response.ErrorException, "The request contains an exception.");
        //        Assert.IsTrue(response.Headers.Any(h => h.Name == "Content-Type" && h.Value.ToString() == "application/text"), "The response does not contains the header.");
        //    }
        //}

        //[TestMethod]
        //[TestCategory("ThreadNotSafe")]
        //public void HHttpServer_ResponseSpecialHeader_Ok()
        //{
        //    using (var hserver = new HttpServerMock(TestServerPort))
        //    {
        //        string hostName = Dns.GetHostName();
        //        hserver.SetUpGetExpectation("http://localhost:50000/user/23")
        //            .Response(HttpStatusCode.OK)
        //            .ResponseHeader("Host", hostName);

        //        var restClient = new RestClient(this.serverBaseUrl);
        //        var request = new RestRequest("/user/23");
        //        request.Method = RestSharp.Method.GET;

        //        var response = restClient.Execute(request);
        //        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The respond status code is not the expected.");
        //        Assert.IsNull(response.ErrorException, "The request contains an exception.");
        //        Assert.IsTrue(response.Headers.Any(h => h.Name == "Host" && h.Value.ToString() == hostName), "The response does not contains the header.");
        //    }
        //}
        //#endregion

        //#region VerifyAllRequests
        //[TestMethod]
        //[TestCategory("ThreadNotSafe")]
        //[ExpectedException(typeof(HttpServerCallsVerificationException))]
        //public void HHttpServer_NoExpectationsConfiguredNotImplementedHttpResult_HttpServerCallsVerificationException()
        //{
        //    using (var hserver = new HttpServerMock(TestServerPort))
        //    {
        //        var restClient = new RestClient(this.serverBaseUrl);
        //        var request = new RestRequest("/user/23");
        //        request.AddJsonBody(new { Name = "test", Id = 23 });
        //        request.AddHeader("test", "test1");
        //        request.Method = RestSharp.Method.POST;

        //        var response = restClient.Execute<ResponseTestClass>(request);
        //        Assert.AreEqual(HttpStatusCode.NotImplemented, response.StatusCode, "The respond status code is not the expected.");
        //        Assert.IsNull(response.Data, "The response is not empty.");
        //        Assert.IsNull(response.ErrorException, "The request contains an exception.");

        //        hserver.VerifyAllRequestExpectationsAndUnexpectedRequests();
        //    }
        //}
        //#endregion
    }
}
