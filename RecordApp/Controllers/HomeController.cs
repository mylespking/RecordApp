using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RecordApp.Models;
using RecordsApp.Data.Entities;

namespace RecordApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            // api controller return getNews()
            var news = await GetNews();
            return View(news);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public async Task<IEnumerable<WelcomeNews>> GetNews()
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
                
                var body = await response.Content.ReadAsStringAsync();

                var foobar = JsonConvert.DeserializeObject<IEnumerable<WelcomeNews>>(body);
                return foobar;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
