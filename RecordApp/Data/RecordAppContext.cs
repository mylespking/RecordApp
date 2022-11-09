using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RecordsApp.Data.Entities;

namespace RecordApp.Data
{ 
    public class RecordAppContext : DbContext
    {
        private readonly IConfiguration _config;

        public RecordAppContext(IConfiguration config)
        {
          _config = config;
        }

        // Set Tables in Database
        public DbSet<Album> Album { get; set; }
        
        // Add constructor to configure to database
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_config["ConnectionStrings:RecordAppDb"]);
        }

    }
}
