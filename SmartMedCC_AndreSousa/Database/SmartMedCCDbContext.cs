using Microsoft.EntityFrameworkCore;

using SmartMedCC_AndreSousa.Models;

namespace SmartMedCC_AndreSousa.Database
{
    /// <summary>
    ///     A <see cref="DbContext"/> for the SmartMed Code Challenge database.
    /// </summary>
    public class SmartMedCCDbContext : DbContext
    {
        /// <summary>
        ///     The default .ctor for <see cref="SmartMedCCDbContext"/>.
        /// </summary>
        /// <param name="options"></param>
        public SmartMedCCDbContext(DbContextOptions<SmartMedCCDbContext> options) : base(options) { }

        /// <summary>
        ///     The <see cref="DbSet{TEntity}"/> for <see cref="Medication"/>.
        /// </summary>
        public DbSet<Medication> Medications { get; set; }

        /// <summary>
        ///     Configures the model during its creation, customizing its default behavior.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Medication>().Property(m => m.Timestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
