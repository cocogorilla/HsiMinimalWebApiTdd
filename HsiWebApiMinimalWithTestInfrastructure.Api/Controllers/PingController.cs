using System;
using System.Threading.Tasks;
using System.Web.Http;
using $ext_safeprojectname$.Core.Abstractions;
using $ext_safeprojectname$.Core.Models;
using Thinktecture.IdentityModel.WebApi;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("v1")]
    public class PingController : ApiController
    {
        private readonly IReceiveAsync<PingModel, PongModel> _pingRepository;

        public PingController(IReceiveAsync<PingModel, PongModel> pingRepository)
        {
            if (pingRepository == null) throw new ArgumentNullException(nameof(pingRepository));
            _pingRepository = pingRepository;
        }

        [Route("ping")]
        [ResourceAuthorize("Read", "Ping")]
        public async Task<IHttpActionResult> Ping(PingModel ping)
        {
            try
            {
                var result = await _pingRepository.GetDataAsync(ping);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}