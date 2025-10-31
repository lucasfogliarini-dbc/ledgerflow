using LedgerFlow;
using LedgerFlow.Infrastructure;
using LedgerFlow.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext(configuration);
            services.AddRepositories();
        }

        public static void AddLedgerFlowDbContextCheck(this IServiceCollection services)
        {
            services.AddHealthChecks()
                    .AddCheck<DbContextHealthCheck<LedgerFlowDbContext>>(nameof(LedgerFlowDbContext));
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient<ILedgerSummaryRepository, LedgerSummaryRepository>();
        }
        private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var LedgerFlowConnectionStringKey = "LedgerFlow";
            Console.WriteLine($"Trying to get a database connectionString '{LedgerFlowConnectionStringKey}' from Configuration.");
            var LedgerFlowConnectionString = configuration.GetConnectionString(LedgerFlowConnectionStringKey);
            if (LedgerFlowConnectionString == null)
            {
                Console.WriteLine("LedgerFlow ConnectionString NOT found, using InMemoryDatabase for LedgerFlowDbContext.");
                services.AddDbContext<LedgerFlowDbContext>(options => options.UseInMemoryDatabase(nameof(LedgerFlowDbContext)));
            }
            else
            {
                Console.WriteLine($"Using LedgerFlow ConnectionString for LedgerFlowDbContext.");
                services.AddDbContext<LedgerFlowDbContext>(options => options.UseSqlServer(LedgerFlowConnectionString));
            }
        }
    }
}
