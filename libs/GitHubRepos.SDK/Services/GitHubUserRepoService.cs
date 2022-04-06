using System.Collections.Generic;
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
                var response = await _gitHubApiClient.GetResponseAsync<UserInfoResponse>(username);
                response.Success = true;
                return response;
            }
            catch
            {
                // TODO -> cases where response code isnt just 404
                return new UserInfoResponse
                {
                    Success = false,
                    ErrorMessage = $"No user information found for the username {username}"
                };
            }
        }

        public async Task<RepoInfoResponse> GetRepoInformationAsync(string username)
        {
            var userInfo = await GetUserInformationAsync(username);

            if (userInfo.UserRepoUrl is null)
            {
                return new RepoInfoResponse
                {
                    Success = false,
                    ErrorMessage = $"No user information found for the username {username}"
                };
            }

            var repoResponse = await _gitHubApiClient.GetResponseListAsync<RepoInfo>(userInfo.UserRepoUrl);

            if (!(repoResponse is null) && repoResponse.Any())
            {
                return new RepoInfoResponse 
                { 
                    Success = true,
                    RepoInfos = repoResponse
                };
            }

            return new RepoInfoResponse
            {
                Success = false,
                ErrorMessage = $"No repositories can be found for the username {username}"
            };
        }

        public async Task<RepoInfoResponse> GetTop5RepoInformationAsync(string username)
        {
            var repoInfo = await GetRepoInformationAsync(username);

            if (repoInfo.Success)
            {
                var top5 = GetTop5Repos(repoInfo);
                repoInfo.RepoInfos = top5;
                return repoInfo;
            }

            return repoInfo;
        }

        private List<RepoInfo> GetTop5Repos(RepoInfoResponse repoInfo)
        {
            return repoInfo.RepoInfos.OrderByDescending(repo => repo.StarGazerCount)
                .Take(5)
                .ToList();
        }
    }
}
