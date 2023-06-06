using Moq;


namespace PowerplantProduction.Tests.Extensions
{
    public static class MockExtensions
    {
        public static Mock<T> AsMock<T>(this T instance) where T : class
        {
            return Mock.Get(instance);
        }
    }
}
