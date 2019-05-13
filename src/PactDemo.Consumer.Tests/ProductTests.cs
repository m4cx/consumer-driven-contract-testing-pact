using NUnit.Framework;
using PactNet.Mocks.MockHttpService.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PactDemo.Consumer.Tests
{
    [TestFixture]
    public class ProductTests : ConsumerProductPactTestBase
    {
        [SetUp]
        public void SetUp()
        {
            MockProviderService.ClearInteractions();
        }

        [Test]
        public async Task GetProduct_ReturnsExpectedProduct()
        {
            var expectedProductId = Guid.NewGuid();
            var expectedProduct = new Product
            {
                Id = expectedProductId,
                Name = "Test",
                Description = "A product for testing",
                EanCode = null
            };
            MockProviderService
                .Given("A Product with expected structure") // Describe the state the provider needs to setup
                .UponReceiving("a GET request for a single product") // textual description - business case
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = $"/{expectedProductId}",
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = expectedProduct
                });

            var consumer = new ProductClient(MockServerBaseUri);
            var result = await consumer.Get(expectedProductId);

            Assert.AreEqual(expectedProduct.Id, result.Id);
            Assert.AreEqual(expectedProduct.Name, result.Name);

            MockProviderService.VerifyInteractions();
        }
    }
}
