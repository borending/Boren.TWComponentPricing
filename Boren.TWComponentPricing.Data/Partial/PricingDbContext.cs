using Microsoft.EntityFrameworkCore;

namespace Boren.TWComponentPricing.Data
{
    public partial class PricingDbContext : DbContext
    {
        private readonly string _connectionString;

        public PricingDbContext(string connectionString) : base()
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (!string.IsNullOrEmpty(_connectionString))
                    optionsBuilder.UseNpgsql(_connectionString);
                else
                    optionsBuilder.UseNpgsql("Host=127.0.0.1;Database=Pricing;Username=postgres");
            }
        }
    }
}
