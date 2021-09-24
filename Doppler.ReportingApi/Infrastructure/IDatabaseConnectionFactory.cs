using System.Data;
using System.Threading.Tasks;

namespace Doppler.ReportingApi.Infrastructure
{
    public interface IDatabaseConnectionFactory
    {
        Task<IDbConnection> GetConnection();
    }
}
