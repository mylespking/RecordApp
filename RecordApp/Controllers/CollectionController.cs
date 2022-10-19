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
        private readonly RecordAppContext _context;

        public CollectionController(
            ILogger<CollectionController> logger,
            RecordAppContext context
            )
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var results = _context.Album
                .OrderBy(a => a.Name)
                .ToList();

            var lyncresults = from a in _context.Album
                              orderby a.Name
                              select a;

            var dataresults = new List<Album>();

            foreach (var a in results)
            {
                var Data = new Album
                {
                    AlbumId = a.AlbumId,
                    Name = a.Name,
                    Artist = a.Artist,
                    Tracks = a.Tracks,
                    AlbumType = a.AlbumType,
                    ReleaseDate = a.ReleaseDate
                };

                dataresults.Add(Data);

            }

            return View(dataresults);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
