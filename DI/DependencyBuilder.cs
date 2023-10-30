using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SimpleCRM.Common.Interfaces.Services;
using SimpleCRM.Data.DbContexts;
using SimpleCRM.Data.Interfaces;
using SimpleCRM.Data.Interfaces.Repositories;
using SimpleCRM.Data.Repositories;
using SimpleCRM.Logic.Services;

namespace SimpleCRM.DI
{
    public class DependencyBuilder
    {
        private readonly IConfiguration configuration;
        public DependencyBuilder(IConfiguration configuration) 
        {
            this.configuration = configuration;
        }

        public void Build(IServiceCollection services)
        {
            //Configure Automapped To Scan All Profiles
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Add The DbContext With The Connection String (For Dev This Is From The appSettings.Development, However For Prod Should Be In A Windows Environment Variable
            services.AddDbContext<MembershipContext>(dbContextOptions => dbContextOptions.UseSqlite(configuration["ConnectionStrings:SimpleCRMConectionString"]));

            //Members
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IMemberService, MemberService>();
        }
    }
}