using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HttpServerMock.Common;
using HttpServerMock.Common.Model;
using Microsoft.Owin;

namespace HttpServerMock
{
    /// <summary>
    /// 
    /// </summary>
    public class RequestMapper : IRequestMapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contextRequest"></param>
        /// <returns></returns>
        public Request Map(IOwinRequest contextRequest)
        {
            var request = new Request
            {
                Method = MapMethod(contextRequest.Method),
                Uri = contextRequest.Uri,
                Content = MapContent(contextRequest.ContentType, contextRequest.Body)
            };

            MapHeaders(contextRequest.Headers, request.Headers);
            return request;
        }

        private IContent MapContent(string contentType, Stream contextRequestBody)
        {
            using (var targetStream = new MemoryStream())
            {
                contextRequestBody.CopyTo(targetStream);
                var binaryContent = targetStream.ToArray();
                if (Helper.IsJsonRequest(contentType))
                {
                    return new JsonContent(Encoding.UTF8.GetString(binaryContent));
                }
                else if(Helper.IsXmlRequest(contentType))
                {
                    return new StringContent(Encoding.UTF8.GetString(binaryContent));
                }

                return new BinaryContent(binaryContent);
            }
        }

        private void MapHeaders(IHeaderDictionary contextRequestHeaders, IDictionary<string, IEnumerable<string>> target)
        {
            foreach (var header in contextRequestHeaders)
            {
                target[header.Key] = header.Value;
            }
        }

        private Method MapMethod(string contextRequestMethod)
        {
            Enum.TryParse(contextRequestMethod, out Method method);
            return method;
        }
    }
}