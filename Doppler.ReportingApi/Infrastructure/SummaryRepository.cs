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
                var dummyDatabaseQuery = @"
                SELECT
                        T.TotalSentEmails,
                        T.DistinctOpenedMailCount as TotalOpenClicks,
                        ISNULL(( T.DistinctOpenedMailCount / NULLIF (T.SoftBouncedMailCount, 0 ) + NULLIF (T.UnopenedMailCount, 0 )), 0) ClickThroughRate
                FROM (
                    SELECT
                    SUM(Campaign.AmountSentSubscribers) AS TotalSentEmails,
                    SUM(Campaign.DistinctOpenedMailCount) AS DistinctOpenedMailCount,

                    SUM(Campaign.SoftBouncedMailCount) AS SoftBouncedMailCount,
                    SUM(Campaign.UnopenedMailCount) AS UnopenedMailCount

                    FROM [user]
                        INNER JOIN Campaign WITH (NOLOCK) on [user].iduser = Campaign.IdUser
                    WHERE
                        Campaign.Status = 5 AND --SENT
                        Campaign.IdTestCampaign IS NULL AND --Exclude test campaigns
                        Campaign.IdScheduledTask IS NULL AND --Exclude automations
                        Email = @userName AND
                        Campaign.UTCSentDate >= @startDate AND
                        Campaign.UTCSentDate < @endDate
                ) T";

                var results = await connection.QueryAsync<CampaignsSummary>(dummyDatabaseQuery, new { userName, startDate, endDate });
                var result = results.SingleOrDefault();
                result = result == null ? new CampaignsSummary() : result;
                result.StartDate = startDate;
                result.EndDate = endDate;
                return result;
            }
        }

        public async Task<SubscribersSummary> GetSubscribersSummaryByUserAsync(string userName, DateTime startDate, DateTime endDate)
        {
            using (var connection = await _connectionFactory.GetConnection())
            {
                var dummyDatabaseQuery = @"
                SELECT
                    (SELECT SUM(S.Amount) FROM ViewSubscribersByStatusXUserAmount S INNER JOIN [User] on [User].idUser = S.IdUser  WHERE [User].Email = @userName) AS TotalSubscribers,
                    COUNT(1) as NewSubscribers,
                    COUNT(CASE WHEN  S.IdSubscribersStatus = 8 THEN 1 END) AS RemovedSubscribers
                FROM Subscriber S
                    INNER JOIN [User] on S.IdUser = [User].idUser
                WHERE [User].Email = @userName AND
                    S.UTCCreationDate >= @startDate AND
                    S.UTCCreationDate < @endDate";

                var results = await connection.QueryAsync<SubscribersSummary>(dummyDatabaseQuery, new { userName, startDate, endDate });
                var result = results.SingleOrDefault();
                result = result == null ? new SubscribersSummary() : result;
                result.StartDate = startDate;
                result.EndDate = endDate;
                return result;
            }
        }

        public async Task<SystemUsageSummary> GetSystemUsageAsync(string accountName)
        {
            using (var connection = await _connectionFactory.GetConnection())
            {
                var databaseQuery = @"
                SELECT
                    HasCampaignCreated AS HasCampaingsCreated,
                    HasListCreated AS HasListsCreated,
                    HasCampaignSent AS HasCampaingsSent
                FROM dbo.[User]
                WHERE Email = @accountName";

                var results = await connection.QueryAsync<SystemUsageSummary>(databaseQuery, new { accountName });
                var result = results.SingleOrDefault();

                return result == null ? new SystemUsageSummary() : result;
            }
        }
    }
}
