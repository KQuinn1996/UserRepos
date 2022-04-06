using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UserRepos.Models
{
    public class UserReposViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Username { get; set; }
        public string Location { get; set; }
        public string AvatarImage { get; set; }
        public List<GitHubRepo> GitHubRepos { get; set; }
    }

    public class GitHubRepo
    {
        public string Name { get; set; }

        public string RepoUrl { get; set; }

        public string Description { get; set; }

        public int StarGazerCount { get; set; }
    }
}