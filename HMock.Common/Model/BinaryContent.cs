using System.IO;
using System.Linq;

namespace HttpServerMock.Common.Model
{
    public class BinaryContent : IContent
    {
        public BinaryContent(byte[] content)
        {
            Content = content;
        }

        public byte[] Content { get; }

        protected bool Equals(BinaryContent other)
        {
            return this.Content.SequenceEqual(other.Content);
        }

        public bool Equals(IContent other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != this.GetType()) return false;
            return Equals((BinaryContent)other);
        }

        public void Write(Stream stream)
        {
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(this.Content);
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BinaryContent) obj);
        }

        public override int GetHashCode()
        {
            return (this.Content != null ? this.Content.GetHashCode() : 0);
        }
    }
}