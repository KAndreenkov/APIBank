using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIBank.Models
{
    public class TestAppContext: DbContext
    {
        public TestAppContext(DbContextOptions<TestAppContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<CurrencyItem> CurrencyItems { get; set; }
        public DbSet<PersonName> PersonNames { get; set; }
        public DbSet<BankRecord> BankRecords { get; set; }
    }
}
