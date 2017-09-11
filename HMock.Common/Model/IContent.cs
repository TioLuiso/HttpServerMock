using System;
using System.IO;

namespace HttpServerMock.Common.Model
{
    public interface IContent : IEquatable<IContent>
    {
        void Write(Stream stream);
    }
}