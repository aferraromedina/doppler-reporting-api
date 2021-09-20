using Doppler.ReportingApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doppler.ReportingApi.Controllers
{
    [ApiController]
    public class SummaryController
    {
        /// <summary>
        /// Return an object summarizing the campaingns performance of an user
        /// </summary>
        /// <param name="accountName">User name</param>
        /// <param name="dateFilter">A basic date range filter</param>
        [HttpGet]
        [Route("{accountName}/summary/campaigns")]
        [ProducesResponseType(typeof(CampaignsSummary), 200)]
        [Produces("application/json")]
        public async Task<CampaignsSummary> Campaigns(string accountName, [FromQuery] BasicDatefilter dateFilter)
        {
            var result = await Task.FromResult(new CampaignsSummary()
            {
                ClickThroughRate = 0.1M,
                TotalOpenClicks = 100,
                TotalSentEmails = 1000,
                StartDate = dateFilter.StartDate,
                EndDate = dateFilter.EndDate
            });

            return result;
        }

        /// <summary>
        /// Return an object summarizing the subscribers of an user
        /// </summary>
        /// <param name="accountName">User name</param>
        /// <param name="dateFilter">A basic date range filter </param>
        [HttpGet]
        [Route("{accountName}/summary/subscribers")]
        [ProducesResponseType(typeof(SubscribersSummary), 200)]
        [Produces("application/json")]
        public async Task<SubscribersSummary> Subscribers(string accountName, [FromQuery] BasicDatefilter dateFilter)
        {
            var result = await Task.FromResult(new SubscribersSummary()
            {
                TotalSubscribers = 1000,
                NewSubscribers = 350,
                RemovedSubscribers = 15,
                StartDate = dateFilter.StartDate,
                EndDate = dateFilter.EndDate
            });

            return result;
        }


    }
}
