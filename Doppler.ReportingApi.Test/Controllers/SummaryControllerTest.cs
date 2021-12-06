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
        const string TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518 = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOjEyMywidW5pcXVlX25hbWUiOiJ0ZXN0MUB0ZXN0LmNvbSIsInJvbGUiOiJVU0VSIiwiZXhwIjoyMDAwMDAwMDAwfQ.E3RHjKx9p0a-64RN2YPtlEMysGM45QBO9eATLBhtP4tUQNZnkraUr56hAWA-FuGmhiuMptnKNk_dU3VnbyL6SbHrMWUbquxWjyoqsd7stFs1K_nW6XIzsTjh8Bg6hB5hmsSV-M5_hPS24JwJaCdMQeWrh6cIEp2Sjft7I1V4HQrgzrkMh15sDFAw3i1_ZZasQsDYKyYbO9Jp7lx42ognPrz_KuvPzLjEXvBBNTFsVXUE-ur5adLNMvt-uXzcJ1rcwhjHWItUf5YvgRQbbBnd9f-LsJIhfkDgCJcvZmGDZrtlCKaU1UjHv5c3faZED-cjL59MbibofhPjv87MK8hhdg";

        public SummaryControllerTest(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
        }

        [Theory]
        [InlineData("test1@test.com", TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518)]
        public async Task Get_summary_campaigns_should_return_valid_response(string userName, string token)
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
            var request = new HttpRequestMessage(HttpMethod.Get, $"{userName}/summary/campaigns?startDate={startDate.ToLongDateString()}&endDate={endDate.ToLongTimeString()}")
            {
                Headers = { { "Authorization", $"Bearer {token}" } }
            };

            // Act
            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
            Assert.NotNull(content);
        }

        [Theory]
        [InlineData("test1@test.com", TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518)]
        public async Task Get_summary_subscribers_should_return_valid_response(string userName, string token)
        {
            // Arrange
            var mockConnection = new Mock<DbConnection>();

            mockConnection
                .SetupDapperAsync(c => c.QueryAsync<SubscribersSummary>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(Enumerable.Empty<SubscribersSummary>());

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.SetupConnectionFactory(mockConnection.Object);
                });

            }).CreateClient(new WebApplicationFactoryClientOptions());

            var startDate = DateTime.Today.AddDays(-30);
            var endDate = DateTime.Today;
            var request = new HttpRequestMessage(HttpMethod.Get, $"/{userName}/summary/subscribers?startDate={startDate.ToLongDateString()}&endDate={endDate.ToLongTimeString()}")
            {
                Headers = { { "Authorization", $"Bearer {token}" } }
            };

            // Act
            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(content);
        }

        [Theory]
        [InlineData("test1@test.com", TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518)]
        public async Task Get_system_usage_should_return_valid_response(string userName, string token)
        {
            // Arrange
            var mockConnection = new Mock<DbConnection>();

            mockConnection
                .SetupDapperAsync(c => c.QueryAsync<SystemUsageSummary>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(Enumerable.Empty<SystemUsageSummary>());

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.SetupConnectionFactory(mockConnection.Object);
                });

            }).CreateClient(new WebApplicationFactoryClientOptions());

            // Act
            var request = new HttpRequestMessage(HttpMethod.Get, $"/{userName}/summary/system-usage")
            {
                Headers = { { "Authorization", $"Bearer {token}" } }
            };
            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(content);
        }
    }
}
