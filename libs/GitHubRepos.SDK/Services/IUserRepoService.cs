using System.Threading.Tasks;
using UserRepos.SDK.Responses;

namespace UserRepos.SDK.Services
{
    public interface IUserRepoService
    {
        Task<UserInfoResponse> GetUserInformationAsync(string username);
        Task<RepoInfoResponse> GetRepoInformationAsync(string repoUrl);
        Task<RepoInfoResponse> GetTop5RepoInformationAsync(string repoUrl);
    }
}
