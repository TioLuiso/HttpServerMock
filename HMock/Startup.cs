using Owin;

namespace HttpServerMock
{
    /// <summary>
    /// Configuration for Owin application
    /// </summary>
    public class Startup
    {
        private readonly MockProcessor mockProcessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mockProcessor"></param>
        public Startup(MockProcessor mockProcessor)
        {
            this.mockProcessor = mockProcessor;
        }
        /// <summary>
        /// Configuration of Owin application
        /// </summary>
        /// <param name="builder">App builder</param>
        public void Configuration(IAppBuilder builder)
        {
            builder.Use(this.mockProcessor);
        }
    }
}