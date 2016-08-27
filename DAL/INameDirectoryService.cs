using Microsoft.Extensions.Logging;
using NameDirectoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NameDirectoryService.DAL
{
    interface INameDirectoryService
    {
        List<NameDirectory> getAllRows(ILogger logger);

        void addNameDirectory(NameDirectory nd);

        void deleteNameDirectoryById(int id);

        void deleteAll();
    }
}
