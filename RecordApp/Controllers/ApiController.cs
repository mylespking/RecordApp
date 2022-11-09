using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RecordApp.Data;
using RecordApp.Models;
using RecordsApp.Data.Entities;

namespace RecordApp.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ApiController : Controller
    {
        private readonly IRecordAppRepository _repository;
        private readonly ILogger<ApiController> _logger;

        public ApiController(IRecordAppRepository repository, ILogger<ApiController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // API to get opening page music news from music-news-API
        [HttpGet]
        public async Task<IEnumerable<News>> GetNews()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://music-news-api.p.rapidapi.com/news"),
                Headers =
                {
                    { "X-RapidAPI-Key", "ec94972663msh845a225b632210ap19fd3djsnb875add99aa7" },
                    { "X-RapidAPI-Host", "music-news-api.p.rapidapi.com" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                var deserializedData = JsonConvert.DeserializeObject<IEnumerable<News>>(response.ToString());
                return deserializedData;   
            }
        }

        // API to search for albums on the spotify database using their API
        public async Task<dynamic> SpotifySearch(string searchString)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://spotify23.p.rapidapi.com/search/?q=" + searchString + "&type=albums&offset=0&limit=20&numberOfTopResults=5"),
                Headers =
                {
                    { "X-RapidAPI-Key", "ec94972663msh845a225b632210ap19fd3djsnb875add99aa7" },
                    { "X-RapidAPI-Host", "spotify23.p.rapidapi.com" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                return response;
            }
        }

        // API to get data about specific albums for the View Album page
        public async Task<dynamic> GetAudioDBApiData(string albumName)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://theaudiodb.p.rapidapi.com/searchalbum.php?a=" + albumName),
                Headers =
                {
                    { "X-RapidAPI-Key", "ec94972663msh845a225b632210ap19fd3djsnb875add99aa7" },
                    { "X-RapidAPI-Host", "theaudiodb.p.rapidapi.com" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                return response;
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
