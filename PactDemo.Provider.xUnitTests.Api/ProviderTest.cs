using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PactNet;
using PactNet.Infrastructure.Outputters;
using Xunit;
using Xunit.Abstractions;

namespace PactDemo.Provider.xUnitTests.Api
{
    public class ProviderTest
    {
        private IWebHost _host;
        private const string StateServiceUri = "http://localhost:4321";
        private readonly ITestOutputHelper _output;

        public ProviderTest(ITestOutputHelper output)
        {
            // var factory = WebHost.CreateDefaultBuilder(null)
            //     .UseStartup<TestStartup>();

            // var hostBuilder = Host.CreateDefaultBuilder()
            //     .ConfigureWebHostDefaults(webBuilder => webBuilder
            //         .UseContentRoot("/home/rramos/git/PactDemo/PactDemo.Provider")
            //         .UseUrls(ServiceUri)
            //         .UseStartup<TestStartup>());

            // var hostBuilder = factory.UseUrls(StateServiceUri).UseStartup<TestStartup>();
            //
            // _host = hostBuilder.Build();
            _output = output;
        }

        [Fact]
        public void EnsureSomethingApiHonoursPactWithConsumer()
        {
            //Arrange
            var config = new PactVerifierConfig
            {
                Outputters =
                    new
                        List<IOutput> //NOTE: We default to using a ConsoleOutput, however xUnit 2 does not capture the console output, so a custom outputter is required.
                        {
                            new XUnitOutput(_output)
                        },
                // CustomHeaders = new Dictionary<string, string>
                // {
                //     {"Authorization", "Basic VGVzdA=="}
                // }, //This allows the user to set request headers that will be sent with every request the verifier sends to the provider
                Verbose = true, //Output verbose verification logs to the test output
            };


            //await _host.StartAsync();

            try
            {
                // using (WebApp.Start<TestStartup>(serviceUri))
                // {
                //Act / Assert
                IPactVerifier pactVerifier = new PactVerifier(config);
                pactVerifier
                    //.ProviderState($"{StateServiceUri}/provider-states")
                    .ServiceProvider("Forecast API", "http://localhost:5000")
                    .HonoursPactWith("Consumer")
                    // .PactUri("..\\..\\..\\Consumer.Tests\\pacts\\consumer-something_api.json")
                    .PactUri(
                        "/home/rramos/git/PactDemo/PactDemo.Consumer.NUnitTests.Contract/bin/Debug/pacts/consumer-forecast_api.json")
                    //or
                    // .PactUri(
                    //     "http://pact-broker/pacts/provider/Something%20Api/consumer/Consumer/latest") //You can specify a http or https uri
                    // //or
                    // .PactUri("http://pact-broker/pacts/provider/Something%20Api/consumer/Consumer/latest",
                    //     new PactUriOptions("someuser",
                    //         "somepassword")) //You can also specify http/https basic auth details
                    // //or
                    // .PactUri("http://pact-broker/pacts/provider/Something%20Api/consumer/Consumer/latest",
                    //     new PactUriOptions("sometoken")) //Or a bearer token
                    // //or (if you're using the Pact Broker, you can use the various different features, including pending pacts)
                    // .PactBroker("http://pact-broker", uriOptions: new PactUriOptions("sometoken"), enablePending: true,
                    //     consumerVersionTags: new List<string> {"master"},
                    //     providerVersionTags: new List<string> {"master"},
                    //     consumerVersionSelectors: new List<VersionTagSelector>
                    //         {new VersionTagSelector("master", false, true)})
                    .Verify();
            }
            finally
            {
                //await _host.StopAsync();
                //_host.Dispose();
            }
        }
    }
}