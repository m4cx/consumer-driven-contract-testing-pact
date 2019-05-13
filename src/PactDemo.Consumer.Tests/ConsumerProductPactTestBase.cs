using NUnit.Framework;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace PactDemo.Consumer.Tests
{
    public abstract class ConsumerProductPactTestBase
    {
        public int MockServerPort { get; } = 9222;

        public string MockServerBaseUri { get { return $"http://localhost:{MockServerPort}"; } }

        public IMockProviderService MockProviderService { get; private set; }

        public PactBuilder PactBuilder { get; private set; }

        [SetUp]
        public void Setup()
        {
            // Create a PactBuilder instance
            var pactConfig = new PactConfig()
            {
                SpecificationVersion = "2.0.0",
                PactDir = @"..\..\pacts",
                LogDir = @"..\..\logs"
            };
            PactBuilder = new PactBuilder(pactConfig);

            PactBuilder.ServiceConsumer(typeof(Product).Assembly.GetName().Name).HasPactWith("PactDemo.Provider");

            MockProviderService = PactBuilder.MockService(MockServerPort);
        }

        [TearDown]
        public void TearDown()
        {
            PactBuilder.Build();
        }
    }
}
