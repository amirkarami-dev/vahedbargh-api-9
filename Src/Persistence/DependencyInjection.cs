using Coreapi.Domain.AggregatesModel.BuildingTariffAgg;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Coreapi.Domain.AggregatesModel.ErtTariffAgg;
using Coreapi.Domain.AggregatesModel.ExecutorAgg;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using Coreapi.Domain.AggregatesModel.GeoAgg;
using Coreapi.Domain.AggregatesModel.PanelMakerAgg;
using Coreapi.Domain.AggregatesModel.QuarterIncrease;
using Coreapi.Domain.AggregatesModel.QuarterTariffAgg;
using Coreapi.Domain.AggregatesModel.RoutAgg;
using Coreapi.Domain.AggregatesModel.SupportAgg;
using Coreapi.Domain.AggregatesModel.UserFileAgg;
using Coreapi.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coreapi.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CoreapiDbContext>(options =>
                options.UseSqlServer(configuration["ConnectionString"], o => o.UseNetTopologySuite()));

            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<ICommentEngFormRepository, CommentEngFormRepository>();
            services.AddScoped<ICheckListFormRepository, CheckListFormRepository>();




            services.AddScoped<IRoutRepository, RouteRepository>();
            services.AddScoped<IElectProjectRepository, ElectProjectRepository>();
            services.AddScoped<IBuildingTariffRepository, BuildingTariffRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IExecutorRepository, ExecutorRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IElectProjectFileRepository, ElectProjectFileRepository>();
            services.AddScoped<IBankTransactionRepository, BankTransactionRepository>();
            services.AddScoped<IEngineerRepository, EngineerRepository>();
            services.AddScoped<IEngineerHistoryRepository, EngineerHistoryRepository>();
            services.AddScoped<IElectProjectProcessRepository, ElectProjectProcessRepository>();
            services.AddScoped<IUserFileRepository, UserFileRepository>();
            services.AddScoped<IQuarterTariffRepository, QuarterTariffRepository>();
            services.AddScoped<ISupportRepository, SupportRepository>();
            services.AddScoped<ISupportMessageRepository, SupportMessageRepository>();
            services.AddScoped<ISupportFileRepository, SupportFileRepository>();
            services.AddScoped<IEngPaymentListRepository, EngPaymentListRepository>();
            services.AddScoped<IEngPaymentTaskRepository, EngPaymentTaskRepository>();
            services.AddScoped<IElectProjectErtFormRepository, ElectProjectErtFormRepository>();
            services.AddScoped<IPanelMakerRepository,PanelMakerRepository>();
            services.AddScoped<IErtTariffRepository, ErtTariffRepository>();
            services.AddScoped<ICheckListEdcRepository, CheckListEdcRepository>();
            services.AddScoped<IEngQuotaBurnRepository, EngQuotaBurnRepository>();
            services.AddScoped<IQuarterIncreaseRepository, QuarterIncreaseRepository>();
            services.AddScoped<ISectionRepository, SectionRepository>();



            return services;
        }
    }
}
