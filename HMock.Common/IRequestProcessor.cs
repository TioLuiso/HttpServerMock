using System.Threading.Tasks;
using HttpServerMock.Common.Model;

namespace HttpServerMock.Common
{
    public interface IRequestProcessor
    {
        Task<Response> SendAsync(Request request);
    }
}