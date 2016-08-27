using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace NameDirectoryService.DAL
{
    class NameDirectoryServiceFactory
    {
        private static NameDirectoryServiceFactory service = new NameDirectoryServiceFactory();

        public String driverClassName { get; set; }

        static public INameDirectoryService CreateService(NameDirectoryDbContext dbContext)
        {
            {
                return new NameDirectoryServiceDb(dbContext);
            }
            throw new Exception("Unknown Database Type");
        }
    }
}
