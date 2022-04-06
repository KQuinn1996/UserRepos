using Newtonsoft.Json;

namespace UserRepos.SDK.Responses
{
    public class UserInfoResponse : IResponse
    {
        [JsonProperty("repos_url")]
        public string UserRepoUrl { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("avatar_url")]
        public string AvatarImage { get; set; }
    }
}

