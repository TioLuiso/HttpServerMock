namespace HttpServerMock.Common.Model
{
    public class StringContent : IContent
    {
        public StringContent(string content)
        {
            Content = content;
        }

        public string Content { get; }
    }
}