using FluentAssertions;
using System.Net.Http;
using System.Threading.Tasks;
using UserRepos.SDK.Clients;
using UserRepos.SDK.Responses;
using UserRepos.SDK.Services;
using Xunit;

namespace UserRepos.SDK.Tests.Unit
{
    public class GitHubUserRepoServiceTests
    {
        private readonly GitHubUserRepoService _sut;
        private readonly GitHubClient _gitHubClient;

        public GitHubUserRepoServiceTests()
        {
            _gitHubClient = new GitHubClient(new HttpClient());
            _sut = new GitHubUserRepoService(_gitHubClient);
        }

        #region GetUserInformation Tests

        [Theory]
        [InlineData("robconery")]
        [InlineData("")]
        public async Task GetUserInformationAsync_ShouldReturnUserInfoResponse_WhenRequestSent(string username)
        {
            // Act
            var response = await _sut.GetUserInformationAsync(username);

            // Assert
            response.Should().BeOfType(typeof(UserInfoResponse));
        }

        [Fact]
        public async Task GetUserInformationAsync_ShouldReturnUserInfo_WhenUsernameExists()
        {
            // Arrange
            var username = "robconery";
            var expectedInfo = new UserInfoResponse
            {
                Success = true,
                ErrorMessage = null,
                UserRepoUrl = "https://api.github.com/users/robconery/repos",
                AvatarImage = "https://avatars.githubusercontent.com/u/78586?v=4",
                Location = "Honolulu, HI"
            };

            // Act
            var response = await _sut.GetUserInformationAsync(username);

            // Assert
            response.Should().BeEquivalentTo(expectedInfo);
        }

        [Fact]
        public async Task GetUserInformationAsync_ShouldReturnUnsuccesfulMessage_WhenUsernameDoesNotExist()
        {
            // Arrange
            var username = "nonexistentusername-578jxk7698";
            var expectedInfo = new UserInfoResponse
            {
                Success = false,
                ErrorMessage = $"No user information found for the username {username}",
                UserRepoUrl = null,
                AvatarImage = null,
                Location = null
            };

            // Act
            var response = await _sut.GetUserInformationAsync(username);

            // Assert
            response.Should().BeEquivalentTo(expectedInfo);
        }

        #endregion

        #region GetRepoInformation Tests

        [Fact]
        public async Task GetRepoInformationAsync_ShouldReturnListOfRepos_WhenReposExist()
        {
            // Arrange
            var username = "robconery";

            // Act
            var response = await _sut.GetRepoInformationAsync(username);

            // Assert
            response.Should().BeOfType(typeof(RepoInfoResponse));
            response.RepoInfos.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetRepoInformationAsync_ShouldReturnNoRepoExistsMessage_WhenUsernameHasNoRepos()
        {
            // Arrange
            var username = "KQuinn1996";
            var expectedInfo = new RepoInfoResponse
            {
                Success = false,
                ErrorMessage = $"No repositories can be found for the username {username}",
                RepoInfos = null
            };

            // Act
            var response = await _sut.GetRepoInformationAsync(username);

            // Assert
            response.Should().BeOfType(typeof(RepoInfoResponse));
            response.Should().BeEquivalentTo(expectedInfo);
        }

        [Fact]
        public async Task GetRepoInformationAsync_ShouldReturnNoUserExistsMessage_WhenUsernameDoesNotExist()
        {
            // Arrange
            var username = "nonexistentusername-578jxk7698";
            var expectedInfo = new RepoInfoResponse
            {
                Success = false,
                ErrorMessage = $"No user information found for the username {username}",
                RepoInfos = null
            };

            // Act
            var response = await _sut.GetRepoInformationAsync(username);

            // Assert
            response.Should().BeOfType(typeof(RepoInfoResponse));
            response.Should().BeEquivalentTo(expectedInfo);
        }

        #endregion

        #region GetTop5RepoInformation Tests

        [Fact]
        public async Task GetTop5RepoInformation_ShouldReturnTop5ReposWithHighestStarCount_WhenUserHasRepos()
        {
            // Arrange
            var username = "robconery";

            // Act
            var response = await _sut.GetTop5RepoInformationAsync(username);

            // Assert
            response.Should().BeOfType(typeof(RepoInfoResponse));
            response.RepoInfos.Should().BeInDescendingOrder(x => x.StarGazerCount)
                .Should();
        }

        #endregion

    }
}