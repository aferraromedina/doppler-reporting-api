using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doppler.ReportingApi.Infrastructure
{
    public interface IDopplerDatabaseSettings
    {
        string GetSqlConnectionString();
    }
}
