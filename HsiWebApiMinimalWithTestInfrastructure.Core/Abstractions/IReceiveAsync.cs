using System.Threading.Tasks;

namespace $safeprojectname$.Abstractions
{
    public interface IReceiveAsync<in TQuery, TResponse>
    {
        Task<TResponse> GetDataAsync(TQuery searchQuery);
    }
}
