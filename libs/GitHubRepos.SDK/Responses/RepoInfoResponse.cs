using Newtonsoft.Json;
using System.Collections.Generic;

namespace UserRepos.SDK.Responses
{
    public class RepoInfoResponse : ResponseBase
    {
        public List<RepoInfo> RepoInfos { get; set; }
    }

    public class RepoInfo 
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("html_url")]
        public string RepoUrl { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("stargazers_count")]
        public int StarGazerCount { get; set; }
    }
}
