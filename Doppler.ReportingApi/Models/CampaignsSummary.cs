using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doppler.ReportingApi.Models
{
    public class CampaignsSummary
    {
        /// <summary>
        /// Number of sent emails in a given period of time (Valid, soft and hard bounced)
        /// </summary>
        public int TotalSentEmails { get; set; }
        /// <summary>
        /// Sum of clicks of campaings in a given period of time
        /// </summary>
        public int TotalOpenClicks { get; set; }
        /// <summary>
        /// Ratio betwheen total sent emails and opened mails
        /// </summary>
        public decimal ClickThroughRate { get; set; }
        /// <summary>
        /// Same value provided in request
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Same value provided in request
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}
