using Dapper;
using Doppler.ReportingApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Doppler.ReportingApi.Infrastructure
{
    public class SummaryRepository : ISummaryRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;
        public SummaryRepository(IDatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<CampaignsSummary> GetCampaignsSummaryByUserAsync(string userName, DateTime startDate, DateTime endDate)
        {
            using (var connection = await _connectionFactory.GetConnection())
            {
                var dummyDatabaseQuery = @"SELECT 1000 as TotalSentEmails, 750 as TotalOpenClicks, 0.8 as ClickThroughRate";

                var results = await connection.QueryAsync<CampaignsSummary>(dummyDatabaseQuery);
                var result = results.SingleOrDefault();
                result.StartDate = startDate;
                result.EndDate = endDate;
                return result;
            }
        }

        public async Task<SubscribersSummary> GetSubscribersSummaryByUserAsync(string userName, DateTime startDate, DateTime endDate)
        {
            using (var connection = await _connectionFactory.GetConnection())
            {
                var dummyDatabaseQuery = @"SELECT 1000 as TotalSubscribers, 750 as NewSubscribers, 80 as RemovedSubscribers";

                var results = await connection.QueryAsync<SubscribersSummary>(dummyDatabaseQuery);
                var result = results.SingleOrDefault();
                result.StartDate = startDate;
                result.EndDate = endDate;
                return result;
            }
        }
    }
}
