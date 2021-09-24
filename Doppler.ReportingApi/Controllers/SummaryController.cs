using Doppler.ReportingApi.Infrastructure;
using Doppler.ReportingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doppler.ReportingApi.Controllers
{
    [ApiController]
    public class SummaryController
    {
        private readonly ILogger _logger;
        private readonly ISummaryRepository _summaryRepository;

        public SummaryController(ILogger<SummaryController> logger, ISummaryRepository summaryRepository)
        {
            _logger = logger;
            _summaryRepository = summaryRepository;
        }

        /// <summary>
        /// Return an object summarizing the campaingns performance of an user, if dateFilter is not provided we show last 30 days
        /// </summary>
        /// <param name="accountName">User name</param>
        /// <param name="dateFilter">A basic date range filter, </param>
        /// <remarks>Dates must be valid UtcTime with timezone</remarks>
        [HttpGet]
        [Route("{accountName}/summary/campaigns")]
        [ProducesResponseType(typeof(CampaignsSummary), 200)]
        [Produces("application/json")]
        public async Task<CampaignsSummary> Campaigns(string accountName, [FromQuery] BasicDatefilter dateFilter)
        {
            var startDate = dateFilter.StartDate.HasValue ? dateFilter.StartDate.Value : DateTime.Today.AddDays(-30);
            var endDate = dateFilter.EndDate;
            var result = await _summaryRepository.GetCampaignsSummaryByUserAsync(accountName, startDate, endDate);
            return result;
        }

        /// <summary>
        /// Return an object summarizing the subscribers of an user
        /// </summary>
        /// <param name="accountName">User name</param>
        /// <param name="dateFilter">A basic date range filter </param>
        /// <remarks>Dates must be valid UtcTime with timezone</remarks>
        [HttpGet]
        [Route("{accountName}/summary/subscribers")]
        [ProducesResponseType(typeof(SubscribersSummary), 200)]
        [Produces("application/json")]
        public async Task<SubscribersSummary> Subscribers(string accountName, [FromQuery] BasicDatefilter dateFilter)
        {
            var startDate = dateFilter.StartDate.HasValue ? dateFilter.StartDate.Value : DateTime.Today.AddDays(-30);
            var endDate = dateFilter.EndDate;
            SubscribersSummary result = await _summaryRepository.GetSubscribersSummaryByUserAsync(accountName, startDate, endDate);
            return result;
        }
    }
}
