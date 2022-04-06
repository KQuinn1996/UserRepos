namespace UserRepos.SDK.Responses
{
    public abstract class ResponseBase : IResponse
    {
        public bool Success { get; set; }

        public string ErrorMessage { get; set; }
    }
}
