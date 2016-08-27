using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NameDirectoryService.DAL
{
    public class NameDirContextFactory : IDbContextFactory<NameDirectoryDbContext>
    {

        public NameDirectoryDbContext Create(DbContextFactoryOptions options)
        {
            //var optionsBuilder = new DbContextOptionsBuilder<NameDirectoryDbContext>();
            //optionsBuilder.UseSqlite(@"Filename=Z:\sources\dchq-docker-aspnet-example\NameDirectoryService\wwwroot\data/sqlight_db2.db");

            var driverClass = Environment.GetEnvironmentVariable("database_driverClassName");
            var connectionString = Environment.GetEnvironmentVariable("database_url");

            //ConnectionSettings conn = new ConnectionSettings() { DriverClassName = "sqlight", DefaultConnection = Path.Combine(options.ContentRootPath ,  @"/wwwroot/data/sqlight_db.db") };
            ConnectionSettings conn = new ConnectionSettings() { DriverClassName = driverClass, DefaultConnection = connectionString };

            return new NameDirectoryDbContext(conn, Path.Combine(options.ContentRootPath + @"/wwwroot"), null);
        }

    }
}
