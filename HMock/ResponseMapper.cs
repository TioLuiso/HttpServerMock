using System.IO;
using System.Linq;
using HttpServerMock.Common.Model;
using Microsoft.Owin;

namespace HttpServerMock
{
    /// <summary>
    /// 
    /// </summary>
    public class ResponseMapper : IResponseMapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="context"></param>
        public void MapResponse(Response response, IOwinContext context)
        {
            this.MapHeaders(response, context.Response.Headers);
            this.MapStatusCode(response, context.Response);
            this.mapContent(response, context.Response);
        }

        private void mapContent(Response response, IOwinResponse contextResponse)
        {
            response.Content.Write(contextResponse.Body);
        }

        private void MapHeaders(Response response, IHeaderDictionary responseHeaders)
        {
            foreach (var header in response.Headers)
            {
                responseHeaders.AppendCommaSeparatedValues(header.Key, header.Value.ToArray());
            }
        }

        private void MapStatusCode(Response response, IOwinResponse owinResponse)
        {
            owinResponse.StatusCode = (int)response.StatusCode;
        }
    }
}