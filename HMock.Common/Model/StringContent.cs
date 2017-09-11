using System.IO;

namespace HttpServerMock.Common.Model
{
    public class StringContent : IContent
    {
        public StringContent(string content)
        {
            Content = content;
        }

        public string Content { get; }

        protected bool Equals(StringContent other)
        {
            return string.Equals(this.Content, other.Content);
        }

        public bool Equals(IContent other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != this.GetType()) return false;
            return Equals((StringContent)other);
        }

        public void Write(Stream stream)
        {
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(this.Content);
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((StringContent) obj);
        }

        public override int GetHashCode()
        {
            return (this.Content != null ? this.Content.GetHashCode() : 0);
        }
    }
}