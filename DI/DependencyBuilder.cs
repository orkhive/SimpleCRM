using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SimpleCRM.Data.DbContexts;
using SimpleCRM.Data.Interfaces;

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
        }
    }
}