using FluentAssertions;
using System;
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

        [Fact]
        public async Task GetUserInformationAsync_ShouldReturnUserInfo_WhenUsernameExists()
        {
            // Arrange
            var username = "robconery";
            var expectedInfo = new UserInfoResponse
            {
                UserRepoUrl = "https://api.github.com/users/robconery/repos",
                AvatarImage = "https://avatars.githubusercontent.com/u/78586?v=4",
                Location = "Honolulu, HI"
            };

            // Act
            var response = await _sut.GetUserInformationAsync(username);

            // Assert
            response.Should().BeOfType(typeof(UserInfoResponse));
            response.Should().BeEquivalentTo(expectedInfo);
        }

        [Fact]
        public async Task GetUserInformationAsync_ShouldThrowExceptionWithError_WhenUsernameDoesNotExist()
        {
            // Arrange
            var username = "nonexistentusername-578jxk7698";
            var expectedErrorMessage = $"No user information found for the username {username}";

            // Act
            Func<Task<IResponse>> response = async () => await _sut.GetUserInformationAsync(username);

            // Assert
            await response.Should().ThrowAsync<Exception>().Where(ex => ex.Message == expectedErrorMessage);
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
            var username = "sdasdd";
            var expectedErrorMessage = $"No repositories can be found for the username {username}";

            // Act
            Func<Task<IResponse>> response = async () => await _sut.GetRepoInformationAsync(username);

            // Assert
            await response.Should().ThrowAsync<Exception>().Where(ex => ex.Message == expectedErrorMessage);
        }

        [Fact]
        public async Task GetRepoInformationAsync_ShouldReturnNoUserExistsMessage_WhenUsernameDoesNotExist()
        {
            // Arrange
            var username = "nonexistentusername-578jxk7698";
            var expectedErrorMessage = $"No user information found for the username {username}";

            // Act
            Func<Task<IResponse>> response = async () => await _sut.GetRepoInformationAsync(username);

            // Assert
            await response.Should().ThrowAsync<Exception>().Where(ex => ex.Message == expectedErrorMessage);
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