using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ModelOfThings.Parser.Models;

namespace ModelOfThings.Parser.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<MddApplication> MddApplications { get; set; }
        public DbSet<MddComponent> MddComponents { get; set; }
        public DbSet<MddPropertie> MddProperties { get; set; }
        public DbSet<UmlModel> UmlModels { get; set; }
        public DbSet<UmlUseCase> UmlUseCases { get; set; }
        public DbSet<UmlAssociation> UmlAssociations { get; set; }
        public DbSet<CloudProvider> CloudProviders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var cascadeFKs = builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Cascade;

            var providerConverter = new EnumToStringConverter<Provider>();

            builder
                .Entity<CloudProvider>()
                .Property(e => e.Provider)
                .HasConversion(providerConverter);

            builder.Entity<MddComponent>().Ignore(p => p.MddComponents);

            base.OnModelCreating(builder);
        }
    }
}
