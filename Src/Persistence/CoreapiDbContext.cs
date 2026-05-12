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
using Coreapi.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Coreapi.Persistence
{
    public class CoreapiDbContext : DbContext, IUnitOfWork
    {
        public CoreapiDbContext(DbContextOptions<CoreapiDbContext> options) : base(options)
        {
        }

     
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientArea> ClientAreas { get; set; }
        public DbSet<ClientUserCapture> ClientUserCaptures { get; set; }
        public DbSet<ClientCard> ClientCards { get; set; }
        public DbSet<ClientSetting> ClientSettings { get; set; }
        public DbSet<ClientUserSetting> ClientUserSettings { get; set; }
        public DbSet<ClientUserTraining> ClientUserTrainings { get; set; }
        public DbSet<ClientAnalyzer> ClientAnalyzers { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<ElectProjectProcess> ElectProjectProcesses { get; set; }
        public DbSet<Executor> Executors { get; set; }
        public DbSet<Engineer> Engineer { get; set; }
        public DbSet<EngineerHistory> EngineerHistories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<BuildingTariff> BuildingTariffs { get; set; }
        public DbSet<QuarterTariff> QuarterTariffs { get; set; }
        public DbSet<ElectProjectFile> ElectProjectFiles { get; set; }
        public DbSet<BankTransaction> BankTransactions { get; set; }
        public DbSet<UserFile> UserFiles { get; set; }
        public DbSet<Support> Supports { get; set; }
        public DbSet<SupportMessage> SupportMessages { get; set; }
        public DbSet<SupportFile> SupportFiles { get; set; }
        public DbSet<EngPaymentList> EngPaymentLists { get; set; }
        public DbSet<EngPaymentTask> EngPaymentTasks { get; set; }
        // new
        public DbSet<ElectProject> ElectProjects { get; set; }
        public DbSet<CommentEngForm> CommentEngForms { get; set; }
        public DbSet<CheckListForm> CheckListForms { get; set; }
        public DbSet<ElectProjectErtForm> ElectProjectErtForms { get; set; }
        public DbSet<PanelMaker> PanelMakers { get; set; }
        public DbSet<ErtTariff> ErtTariffs { get; set; }
        public DbSet<CheckListEdc> CheckListEdcs { get; set; }

        public DbSet<EngQuotaBurn> EngQuotaBurns { get; set; }

        public DbSet<QuarterIncrease> QuarterIncreases { get; set; }



        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Section> Sections { get; set; }

        //public int OnGetUserBalance(string userId) => throw new NotSupportedException();




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoreapiDbContext).Assembly);
          

           // GenerateTemplateSeedData(modelBuilder);

          //  GeneratePackage(modelBuilder);

            // modelBuilder.HasDefaultSchema("dbo");
            //base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var currentDate = DateTime.Now;
            foreach (var entry in ChangeTracker.Entries()
                .Where(e => (
                   e.Entity is Client
                || e.Entity is ClientArea
                || e.Entity is Task
                || e.Entity is ElectProjectProcess
                || e.Entity is Executor
                || e.Entity is Engineer
                || e.Entity is EngineerHistory
                || e.Entity is Transaction
                || e.Entity is Invoice
                || e.Entity is BuildingTariff
                || e.Entity is QuarterTariff
                || e.Entity is ElectProjectFile
                || e.Entity is BankTransaction
                || e.Entity is UserFile
                || e.Entity is Support
                || e.Entity is SupportFile
                || e.Entity is SupportMessage
                || e.Entity is EngPaymentList   
                || e.Entity is EngPaymentTask
                || e.Entity is ElectProject
                || e.Entity is CommentEngForm
                || e.Entity is CheckListForm
                || e.Entity is ElectProjectErtForm
                   || e.Entity is PanelMaker 
                   || e.Entity is ErtTariff
                   || e.Entity is CheckListEdc
                   || e.Entity is Province
                   || e.Entity is City
                   || e.Entity is Section

                ) && (e.State == EntityState.Added || e.State == EntityState.Modified)))
            {
                entry.Property("LastModified").CurrentValue = currentDate;
                if (entry.State == EntityState.Added)
                    entry.Property("Created").CurrentValue = currentDate;
            }
            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
