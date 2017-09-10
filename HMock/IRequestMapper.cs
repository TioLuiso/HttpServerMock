using HttpServerMock.Common.Model;
using Microsoft.Owin;

namespace HttpServerMock
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRequestMapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contextRequest"></param>
        /// <returns></returns>
        Request Map(IOwinRequest contextRequest);
    }
}