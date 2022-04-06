using System;
using System.Linq;
using System.Threading.Tasks;
using UserRepos.SDK.Clients;
using UserRepos.SDK.Responses;

namespace UserRepos.SDK.Services
{
    public class GitHubUserRepoService : IUserRepoService
    {
        private readonly IApiClient _gitHubApiClient;

        public GitHubUserRepoService(IApiClient gitHubApiClient)
        {
            _gitHubApiClient = gitHubApiClient;
        }
        public async Task<UserInfoResponse> GetUserInformationAsync(string username)
        {
            try
            {
                return await _gitHubApiClient.GetResponseAsync<UserInfoResponse>(username);
            }
            catch (Exception ex)
            {
                throw new Exception($"No user information found for the username {username}");
            }
        }

        public async Task<RepoInfoResponse> GetRepoInformationAsync(string username)
        {
            var userInfo = await GetUserInformationAsync(username);

            var repoResponse = await _gitHubApiClient.GetResponseListAsync<RepoInfo>(userInfo.UserRepoUrl);

            if (!(repoResponse is null) && repoResponse.Any())
            {
                return new RepoInfoResponse 
                { 
                    RepoInfos = repoResponse
                };
            }
            else
            {
                throw new Exception($"No repositories can be found for the username {username}");
            }
        }

        public async Task<RepoInfoResponse> GetTop5RepoInformationAsync(string username)
        {
            var repoInfo = await GetRepoInformationAsync(username);

            repoInfo.RepoInfos = repoInfo.RepoInfos.OrderByDescending(repo => repo.StarGazerCount)
                .Take(5)
                .ToList(); 

            return repoInfo;
        }
    }
}
