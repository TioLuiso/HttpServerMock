// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="XmlContent.cs" company="TioLuiso">
// //   (c) TioLuiso 2017
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace HttpServerMock.Common.Model
{
    public class XmlContent : IContent
    {
        public XmlContent(XElement content)
        {
            this.Content = content;
        }

        public XElement Content { get;  }


        public void Write(Stream stream)
        {
            using (var writer = XmlWriter.Create(stream, new XmlWriterSettings{Encoding = new UTF8Encoding(false)}))
            {
                this.Content.WriteTo(writer);
            }
        }

        protected bool Equals(XmlContent other)
        {
            if (!this.Content.Name.LocalName.Contains("AnonymousType"))
            {
                return new XNodeEqualityComparer().Equals(this.Content, other.Content);
            }

            foreach (var childElements in this.Content.Elements().Zip(other.Content.Elements(), (thisElement, otherElement) => new { thisElement, otherElement }))
            {
                var comparison = new XNodeEqualityComparer().Equals(childElements.thisElement, childElements.otherElement);
                if (!comparison)
                {
                    return false;
                }
            }

            return true;
        }

        public bool Equals(IContent other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != this.GetType()) return false;
            return Equals((XmlContent)other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((XmlContent)obj);
        }

        public override int GetHashCode()
        {
            return (this.Content != null ? this.Content.GetHashCode() : 0);
        }
    }
}