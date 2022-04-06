using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using UserRepos.SDK.Clients;
using UserRepos.SDK.Services;

namespace UserRepos
{
    public class ContainerConfig
    {
        public static void RegisterContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.Register(c => new HttpClient())
                .As<HttpClient>();

            builder.RegisterType<GitHubClient>()
                .As<IApiClient>()
                .SingleInstance();

            builder.RegisterType<GitHubUserRepoService>()
                .As<IUserRepoService>()
                .SingleInstance();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}