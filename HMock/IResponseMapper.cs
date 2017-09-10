using HttpServerMock.Common.Model;
using Microsoft.Owin;

namespace HttpServerMock
{
    /// <summary>
    /// 
    /// </summary>
    public interface IResponseMapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="context"></param>
        void MapResponse(Response response, IOwinContext context);
    }
}