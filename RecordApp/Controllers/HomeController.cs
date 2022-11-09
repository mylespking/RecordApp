using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Method which opens the home page showing the latest music news using the music news api
        public async Task<IActionResult> IndexAsync()
        {
            // api controller return getNews()
            var news = await GetNews();

            // method to add an image to each news article so page looks better
            foreach(var article in news)
            {
                // generate a random number 
                var rand = new Random();
                // get locally stored images from the images file 
                var imagesInFolder = Directory.GetFiles("wwwroot/lib/images", "*.jpeg");
                // select a random image using the randomly generated number
                var randomImage = imagesInFolder[rand.Next(imagesInFolder.Length)].ToString();
                // remove the file path start (wwwroot) so image can be found 
                article.image = randomImage.Remove(0, 7);
            }

            return View(news);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Method to get data from the news API 
        [HttpGet]
        public async Task<List<WelcomeNews>> GetNews()
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
                var deserializedData = JsonConvert.DeserializeObject<IEnumerable<WelcomeNews>>(body);

                // Filter data to remove articles from some sources as title has html embedded in it which looks horrible
                var newsData = deserializedData.Where(d => d.source != "kerrang" && d.source != "mtv").ToList();

                return newsData;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
