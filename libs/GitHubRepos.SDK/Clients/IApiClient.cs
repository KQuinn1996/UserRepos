using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserRepos.SDK.Clients
{
    public interface IApiClient : IDisposable
    {
        Task<IResponse> GetResponseAsync<IResponse>(string url);
        Task<List<IResponse>> GetResponseListAsync<IResponse>(string url);
    }
}
