using Dapper;
using Doppler.ReportingApi.Models;
using Doppler.ReportingApi.Test.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Moq;
using Moq.Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Doppler.ReportingApi.Controllers
{
    public class SummaryControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _output;

        public SummaryControllerTest(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
        }

        [Theory]
        [InlineData("test@makingsense.com")]
        public async Task Get_summary_campaigns_should_return_valid_response(string userName)
        {
            // Arrange
            var mockConnection = new Mock<DbConnection>();

            mockConnection
                .SetupDapperAsync(c => c.QueryAsync<CampaignsSummary>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(Enumerable.Empty<CampaignsSummary>());

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.SetupConnectionFactory(mockConnection.Object);
                });

            }).CreateClient(new WebApplicationFactoryClientOptions());

            var startDate = DateTime.Today.AddDays(-30);
            var endDate = DateTime.Today;
            var request = new HttpRequestMessage(HttpMethod.Get, $"{userName}/summary/campaigns?startDate={startDate.ToLongDateString()}&endDate={endDate.ToLongTimeString()}");

            // Act
            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
            Assert.NotNull(content);
        }

        [Theory]
        [InlineData("test@makingsense.com")]
        public async Task Get_summary_usage_should_return_valid_response(string userName)
        {
            // Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions());
            var request = new HttpRequestMessage(HttpMethod.Get, $"/{userName}/summary/system-usage");

            // Act
            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(content);
        }
    }
}
