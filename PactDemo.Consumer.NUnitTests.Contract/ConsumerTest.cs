using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;

namespace PactDemo.Consumer.NUnitTests.Contract
{
    public class Tests
    {
        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;
        private ConsumerMyApiPact _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new ConsumerMyApiPact();
            _mockProviderService = _fixture.MockProviderService;
            _mockProviderService
                .ClearInteractions(); //NOTE: Clears any previously registered interactions before the test is run
            _mockProviderServiceBaseUri = _fixture.MockProviderServiceBaseUri;
        }

        [TearDown]
        public void Cleanup()
        {
            _fixture.Dispose();
        }

        [Test]
        public async Task Test1()
        {
            _mockProviderService
                .UponReceiving("Receber a previsão do tempo para amanhã")
                .With(new ProviderServiceRequest
                {
                    Path = "/WeatherForecast/tomorrow",
                    Method = HttpVerb.Get,
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
                    Body = new 
                    {
                        date = DateTime.Today.AddDays(1),
                        summary = "Chilly",
                        temperatureC = 15,
                        temperatureF = 58
                    }
                });

            var consumer = new WeatherForecastApiClient(_mockProviderServiceBaseUri);

            // Act
            var result = await consumer.GetForecastTomorrow();

            // Assert
            result.Should().BeEquivalentTo(
                new WeatherForecastResponse
                {
                    Date = DateTime.Today.AddDays(1),
                    Summary = "Chilly",
                    TemperatureC = 15,
                    TemperatureF = 58
                }
            );
            
            _mockProviderService.VerifyInteractions();
        }
    }
}