using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doppler.ReportingApi.Models
{
    public class SystemUsageSummary
    {
        public bool HasListsCreated { get; set; }
        public bool HasCampaingsCreated { get; set; }
        ///// <summary>
        ///// Has DKIM/SPF configuration completed and valid
        ///// </summary>
        //public bool HasDomainsReady { get; set; }
        public bool HasCampaingsSent { get; set; }
    }
}
