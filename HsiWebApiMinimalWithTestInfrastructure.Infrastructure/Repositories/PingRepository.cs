using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using $ext_safeprojectname$.Core.Abstractions;
using $ext_safeprojectname$.Core.Models;

namespace $safeprojectname$.Repositories
{
    public class PingRepository : IReceiveAsync<PingModel, PongModel>
    {
        private readonly AppConfig _config;

        public PingRepository(AppConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            _config = config;
        }
        public async Task<PongModel> GetDataAsync(PingModel searchQuery)
        {
            using (var conn = new SqlConnection(_config.ConnectionString))
            {
                var result = await conn.QueryAsync<PongModel>("select pong from ping");
                return result.Single();
            }
        }
    }
}
