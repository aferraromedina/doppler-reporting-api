using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doppler.ReportingApi.Models
{
    public class SubscribersSummary
    {
        public int TotalSubscribers { get; set; }
        public int NewSubscribers { get; set; }
        public int RemovedSubscribers { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
    }
}
