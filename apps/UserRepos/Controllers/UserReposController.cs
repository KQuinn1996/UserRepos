using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using UserRepos.Models;
using System;
using UserRepos.SDK.Services;
using UserRepos.SDK.Responses;

namespace UserRepos.Controllers
{
    [HandleError()]
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
            ViewData["ValidationMessage"] = "";
            ViewData["ErrorMessage"] = "";
            return View();
        }

        // TODO 
        //  - Tests, mocking out client and service
        // https://stackoverflow.com/questions/5021552/how-to-reference-a-css-file-on-a-razor-view

        [HttpPost]
        public async Task<ActionResult> Index(string username)
        {
            ViewData["ValidationMessage"] = "";
            ViewData["ErrorMessage"] = "";

            if (string.IsNullOrEmpty(username))
            {
                ViewData["ValidationMessage"] = "Please enter a username";
                return View();
            }

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

        //// TODO 

        //[HttpPost]
        //public async Task<ActionResult> Index(string username)
        //{
        //    ViewData["ValidationMessage"] = "";
        //    ViewData["ErrorMessage"] = "";

        //    if (string.IsNullOrEmpty(username))
        //    {
        //        ViewData["ValidationMessage"] = "Please enter a username";
        //        return View();
        //    }

        //    var userInfo = await _userRepoService.GetUserInformationAsync(username);
        //    var repoInfo = await _userRepoService.GetTop5RepoInformationAsync(username);

        //    if (repoInfo.Success)
        //        ViewData["RepoData"] = ConvertToView(username, userInfo, repoInfo);
        //    else
        //        ViewData["ErrorMessage"] = repoInfo.ErrorMessage;

        //    return View();
        //}

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