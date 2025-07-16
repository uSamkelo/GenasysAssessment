using GenasysAssessment.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GenasysAssessment.Data
{
    public class ApplicationDataContext : DbContext
    {
        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
    }
}
