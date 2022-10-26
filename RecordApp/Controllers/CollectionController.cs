using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecordApp.Data;
using RecordApp.Models;

namespace RecordApp.Controllers
{
    public class CollectionController : Controller
    {
        private readonly ILogger<CollectionController> _logger;
        private readonly IRecordAppRepository _repository;

        public CollectionController(
            ILogger<CollectionController> logger,
            IRecordAppRepository repository
            )
        {
            _logger = logger;
            _repository = repository;
        }

        public IActionResult Index()
        {
            var albums = _repository.GetAlbums();
            return View(albums);
        }

        public IActionResult Add()
        {
            var albums = _repository.GetAlbums();

            return View(albums);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
