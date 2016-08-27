using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NameDirectoryService.Models;
using NameDirectoryService.DAL;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace NameDirectoryService.Controllers
{
    public class NameDirectoryController : Controller
    {
        ILogger logger { get; set; } // = ApplicationLogging.CreateLogger<Controller>();

        private INameDirectoryService _service = null;

        public NameDirectoryController(IOptions<ConnectionSettings> settings, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger("Logger");
            _service = new NameDirectoryServiceDb(new NameDirectoryDbContext(settings, env.WebRootPath, logger));
            logger.LogInformation("NameDirectoryController ...");
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(_service.getAllRows(logger));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(string FirstName, string LastName)
        {
            if (ModelState.IsValid)
            {
                var newItem = new NameDirectory() { FirstName = FirstName, LastName = LastName, CreatedTimestamp = DateTime.Now.ToString() }; 
                _service.addNameDirectory(newItem);
            }
            return RedirectToAction("Index"); 
        }       

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Remove(int Id)
        {
            if (ModelState.IsValid)
            {
                _service.deleteNameDirectoryById(Id);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult RemoveAll()
        {
            if (ModelState.IsValid)
            {
                _service.deleteAll();
            }
            return RedirectToAction("Index");
        }
    }
}
