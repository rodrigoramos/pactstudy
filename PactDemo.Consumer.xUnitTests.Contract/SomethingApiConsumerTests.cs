using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;

namespace PactDemo.Consumer.xUnitTests.Contract
{
    public class SomethingApiConsumerTests : IDisposable // : IClassFixture<ConsumerMyApiPact>
    {
        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;
        private ConsumerMyApiPact _data;

        // public SomethingApiConsumerTests(ConsumerMyApiPact data)
        public SomethingApiConsumerTests()
        {
            _data = new ConsumerMyApiPact();
            var data = _data;
            _mockProviderService = data.MockProviderService;
            _mockProviderService
                .ClearInteractions(); //NOTE: Clears any previously registered interactions before the test is run
            _mockProviderServiceBaseUri = data.MockProviderServiceBaseUri;
        }

        [Fact]
        public async Task GetSomething_WhenTheTesterSomethingExists_ReturnsTheSomething()
        {
            //Arrange
            _mockProviderService
                .Given("There is a something with id 'tester'")
                .UponReceiving("A GET request to retrieve the something")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/somethings/tester",
                    Headers = new Dictionary<string, object>
                    {
                        {"Accept", "application/json"}
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        {"Content-Type", "application/json; charset=utf-8"}
                    },
                    Body =
                        new //NOTE: Note the case sensitivity here, the body will be serialised as per the casing defined
                        {
                            id = "tester",
                            firstName = "Totally",
                            lastName = "Awesome"
                        }
                }); //NOTE: WillRespondWith call must come last as it will register the interaction

            var consumer = new SomethingApiClient(_mockProviderServiceBaseUri);

            //Act
            var result = await consumer.GetSomething("tester");

            //Assert
            Assert.Equal("tester", result.id);

            _mockProviderService
                .VerifyInteractions(); //NOTE: Verifies that interactions registered on the mock provider are called at least once
        }

        public void Dispose()
        {
            _data?.Dispose();
        }
    }
}