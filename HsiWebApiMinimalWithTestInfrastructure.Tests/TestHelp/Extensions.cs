using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace $safeprojectname$.TestHelp
{
    public static class Extensions
    {
        public static async Task<HttpResponseMessage> Unwrap(this Task<IHttpActionResult> response)
        {
            var result = await (await response).ExecuteAsync(CancellationToken.None);
            return result;
        }

        public static async Task<T> Unwrap<T>(this HttpResponseMessage response)
        {
            var result = await response.Content.ReadAsAsync<T>();
            return result;
        }

        public static async Task<T> Unwrap<T>(this Task<IHttpActionResult> response)
        {
            var result = await response.Unwrap();
            return await result.Unwrap<T>();
        }
    }
}
