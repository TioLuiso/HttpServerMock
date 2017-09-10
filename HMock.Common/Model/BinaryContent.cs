namespace HttpServerMock.Common.Model
{
    public class BinaryContent : IContent
    {
        public BinaryContent(byte[] content)
        {
            Content = content;
        }

        public byte[] Content { get; }
    }
}