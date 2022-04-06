using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using UserRepos.Models;
using System;
using UserRepos.SDK.Services;
using UserRepos.SDK.Responses;

namespace UserRepos.Controllers
{
    public class UserReposController : Controller
    {
        private readonly IUserRepoService _userRepoService;

        public UserReposController(IUserRepoService userRepoService)
        {
            _userRepoService = userRepoService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            InitialiseViewData();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(string username)
        {
            InitialiseViewData();

            if (!ValidateUsername(username))
                return View();

            try
            {
                var userInfo = await _userRepoService.GetUserInformationAsync(username);
                var repoInfo = await _userRepoService.GetTop5RepoInformationAsync(username);

                ViewData["RepoData"] = ConvertToView(username, userInfo, repoInfo);
                return View();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        private bool ValidateUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                ViewData["ValidationMessage"] = "Please enter a username";
                return false;
            }

            return true;
        }

        private void InitialiseViewData()
        {
            ViewData["ValidationMessage"] = "";
            ViewData["ErrorMessage"] = "";
        }

        private UserReposViewModel ConvertToView(string username, UserInfoResponse userInfo, RepoInfoResponse repoInfo)
        {
            return new UserReposViewModel
            {
                Username = username,
                Location = userInfo.Location,
                AvatarImage = userInfo.AvatarImage,
                GitHubRepos = repoInfo.RepoInfos.Select(repo => new GitHubRepo
                    {
                        Name = repo.Name,
                        RepoUrl = repo.RepoUrl,
                        Description = repo.Description,
                        StarGazerCount = repo.StarGazerCount,
                    }).ToList()
            };
        }

    }
}