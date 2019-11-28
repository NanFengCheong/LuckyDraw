using LuckyDraw.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuckyDraw.Context
{
    public class LuckyDrawContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Prize> Prizes { get; set; }
        public LuckyDrawContext()
        { }

        public LuckyDrawContext(DbContextOptions<LuckyDrawContext> options)
            : base(options)
        { }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlite("Data Source=luckyDraw.db");
            }
        }
    }
}
