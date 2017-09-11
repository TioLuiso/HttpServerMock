// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="JsonContent.cs" company="TioLuiso">
// //   (c) TioLuiso 2017
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HttpServerMock.Common.Model
{
    public class JsonContent : IContent
    {
        public JsonContent(JToken content)
        {
            this.Content = content;
        }

        public JsonContent(string content)
        {
            this.Content = JToken.Parse(content);
        }

        public JToken Content { get; }

        protected bool Equals(JsonContent other)
        {
            return JToken.DeepEquals(other.Content, this.Content);
        }

        public bool Equals(IContent other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != this.GetType()) return false;
            return Equals((JsonContent)other);
        }

        public void Write(Stream stream)
        {
            using (var writer = new StreamWriter(stream))
            {
                new JsonSerializer().Serialize(writer, this.Content);
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((JsonContent) obj);
        }

        public override int GetHashCode()
        {
            return (this.Content != null ? this.Content.GetHashCode() : 0);
        }
    }
}