using System;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace PactDemo.Consumer.xUnitTests.Contract
{
    public class ConsumerMyApiPact : IDisposable
    {
        public IPactBuilder PactBuilder { get; }
        
        public IMockProviderService MockProviderService { get; }

        public int MockServerPort => 9222;

        public string MockProviderServiceBaseUri => $"http://localhost:{MockServerPort}";

        public ConsumerMyApiPact()
        {
            PactBuilder =
                new PactBuilder(); //Defaults to specification version 1.1.0, uses default directories. PactDir: ..\..\pacts and LogDir: ..\..\logs
            //or
            PactBuilder =
                new PactBuilder(new PactConfig {SpecificationVersion = "2.0.0"}); //Configures the Specification Version
            //or
            PactBuilder =
                new PactBuilder(new PactConfig
                    {PactDir = @"..\pacts", LogDir = @"c:\temp\logs"}); //Configures the PactDir and/or LogDir.

            PactBuilder
                .ServiceConsumer("Consumidor")
                .HasPactWith("Something API 2");

            MockProviderService = PactBuilder.MockService(MockServerPort); //Configure the http mock server
        }

        public void Dispose()
        {
            PactBuilder.Build(); //NOTE: Will save the pact file once finished
        }
    }
}