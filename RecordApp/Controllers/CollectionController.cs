using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

        public async Task<ActionResult> Add(string? query)
        {
            if (!string.IsNullOrEmpty(query))
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://spotify23.p.rapidapi.com/search/?q={query}&type=albums"),
                    Headers =
                {
                    { "X-RapidAPI-Key", "ec94972663msh845a225b632210ap19fd3djsnb875add99aa7" },
                    { "X-RapidAPI-Host", "spotify23.p.rapidapi.com" },
                },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    var foobar = JsonConvert.DeserializeObject<SearchResponse>(body);
                    return View(foobar);
                }
            }

            return View();
        }

        public async Task<ActionResult> ViewAlbum(string albumName, string artist)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://theaudiodb.p.rapidapi.com/searchalbum.php?s={artist}&a={albumName}"),
                Headers =
                    {
                        { "X-RapidAPI-Key", "ec94972663msh845a225b632210ap19fd3djsnb875add99aa7" },
                        { "X-RapidAPI-Host", "theaudiodb.p.rapidapi.com" },
                    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var foobar = JsonConvert.DeserializeObject<AUdatabasesearch>(body);
                return View(foobar);
            }
        }

        public async Task<ActionResult> AddAlbum(string albumName)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
