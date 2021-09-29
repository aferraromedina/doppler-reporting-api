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
                        T.DistinctOpenedMailCount,
                        ISNULL(( T.DistinctOpenedMailCount / NULLIF (T.SendedMailCount,0 )), 0) ClickThroughRate
                FROM (
                    SELECT
                        COUNT(*)  TotalSentEmails,
                        ISNULL(SUM(CASE IdDeliveryStatus WHEN 100 THEN 1 END),0) DistinctOpenedMailCount,
                        COUNT(CASE IdDeliveryStatus WHEN 0 THEN 1
                                                WHEN 1 THEN 1
                                                WHEN 3 THEN 1
                                                WHEN 4 THEN 1
                                                WHEN 5 THEN 1
                                                WHEN 6 THEN 1
                                                WHEN 7 THEN 1   END)  SendedMailCount

                    FROM [user]
                        INNER JOIN Campaign WITH (NOLOCK) on [user].iduser = Campaign.IdUser
                        LEFT JOIN CampaignDeliveriesOpenInfo WITH(NOLOCK)  ON CampaignDeliveriesOpenInfo.IdCampaign = Campaign.IdCampaign
                    WHERE
                        Campaign.Status = 5 AND--SENT
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
                    (SELECT Count(1) FROM Subscriber S INNER JOIN [User] on [User].idUser = S.IdUser  WHERE [User].Email = @userName) AS TotalSubscribers,
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
    }
}
