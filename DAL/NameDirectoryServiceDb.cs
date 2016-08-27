using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NameDirectoryService.Models;
using Microsoft.Extensions.Logging;

namespace NameDirectoryService.DAL
{
    public class NameDirectoryServiceDb : INameDirectoryService
    {
        private NameDirectoryDbContext _context;
        public NameDirectoryServiceDb(NameDirectoryDbContext context)
        {
            _context = context;
        }

        public void addNameDirectory(NameDirectory newItem)
        {
            _context.NameDirectory.Add(newItem);
            _context.SaveChanges();
        }

        public void deleteAll()
        {
            _context.NameDirectory.RemoveRange(_context.NameDirectory);
            _context.SaveChanges();
        }

        public void deleteNameDirectoryById(int id)
        {
            var n = _context.NameDirectory.FirstOrDefault(i => i.ID == id);
            if (n != null)
            {
                _context.NameDirectory.Remove(n);
                _context.SaveChanges();
            }
        }

        public List<NameDirectory> getAllRows(ILogger logger)
        {
            try
            {
                return _context.NameDirectory.ToList();
            }
            catch(Exception ex)
            {
                logger.LogError("getAllRows error: " + ex.Message);
                return null;
            }
        }
    }
}
