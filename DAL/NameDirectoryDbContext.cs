using NameDirectoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace NameDirectoryService.DAL
{
    public class NameDirectoryDbContext : DbContext
    {
        ConnectionSettings connectionSettings;
        string webRootPath;
        ILogger logger;

        public NameDirectoryDbContext(IOptions<ConnectionSettings> settings, string webRootPath, ILogger logger)
        {
            this.connectionSettings = settings.Value;
            this.webRootPath = webRootPath;
            this.logger = logger;
            //Database.Migrate();
               //.SetInitializer<SchoolDBContext>(new CreateDatabaseIfNotExists<SchoolDBContext>());
        }

        public NameDirectoryDbContext(ConnectionSettings connectionSettings, string webRootPath, ILogger logger)
        {
            this.connectionSettings = connectionSettings;
            this.webRootPath = webRootPath;
            this.logger = logger;
            //Database.Migrate();
            //.SetInitializer<SchoolDBContext>(new CreateDatabaseIfNotExists<SchoolDBContext>());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (logger != null)
                logger.LogInformation("Database driver=" + connectionSettings.DriverClassName + ". ConnectionURL=" + connectionSettings.DefaultConnection + ". WebRootPath=" + this.webRootPath);
            else
                Console.WriteLine("Database driver=" + connectionSettings.DriverClassName + ". ConnectionURL=" + connectionSettings.DefaultConnection + ". WebRootPath=" + this.webRootPath);

            if (IfDriverClass("postgre"))
                optionsBuilder.UseNpgsql(connectionSettings.DefaultConnection);
            else if (IfDriverClass("sqlight"))
                optionsBuilder.UseSqlite("FileName=" + this.webRootPath + @"/data/sqlight_db.db");
            else
                optionsBuilder.UseSqlServer(connectionSettings.DefaultConnection);
        }

        protected bool IfDriverClass(string className)
        {
            return connectionSettings.DriverClassName.StartsWith(className, StringComparison.OrdinalIgnoreCase);
        }

        public DbSet<NameDirectory> NameDirectory { get; set; }
    }
}