using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RecordApp.Classes;
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
                    foobar.searchQuery = query;
                    return View(foobar);
                }
            }

            return View();
        }

        public async Task<ActionResult> ViewAlbum(string? artist, string? albumName, int? albumId, string? searchQuery)
        {
            if (albumId != null)
            {
                var desco = _repository.GetAlbum((int)albumId).First();

                var ownAlbum = new ViewAlbum
                {
                    Name = desco.Name,
                    Artist = desco.Artist,
                    ReleaseYear = desco.ReleaseYear,
                    Label  = desco.Label,
                    Score = desco.Score,
                    AlbumThumb = desco.AlbumThumb,
                    AlbumThumbBack = desco.AlbumThumbBack,
                    Description = desco.Description.Replace("$./;$*", System.Environment.NewLine),
                    Genre = desco.Genre,
                    Mood = desco.Mood,
                    Review = desco.Review,
                    Style = desco.Style,
                    SavedToCollection = desco.SavedToCollection
                };

                return View(ownAlbum);
            }

            // Remove (Deluxe Edition) or (Remastered) so that the Audio DB can get info about the album
            if (albumName.ToLower().Contains("deluxe") || albumName.ToLower().Contains("remastered"))
            {
                var firstBracket = albumName.IndexOf("(");
                var cutAmount = albumName.IndexOf(")") - (firstBracket - 1);
                albumName = albumName.Remove(firstBracket, cutAmount);
            }


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

                var mapping = foobar.album.First();

                // add db search for this 
                var toReturn = new ViewAlbum
                {
                    Name = mapping.strAlbum,
                    Artist = mapping.strArtist,
                    ReleaseYear = mapping.intYearReleased,
                    Label = mapping.strLabel,
                    Score = mapping.intScore,
                    AlbumThumb = mapping.strAlbumThumb,
                    AlbumThumbBack = mapping.strAlbumThumbBack,
                    Description = mapping.strDescriptionEN,
                    Genre = mapping.strGenre,
                    Mood = mapping.strMood,
                    Review = mapping.strReview,
                    Style = mapping.strStyle,
                    SavedToCollection = false
                };

                // Add search query for back button logic
                toReturn.SearchQuery = searchQuery;

                return View(toReturn);
            }
        }

        public async Task<ActionResult> AddAlbum(string artist, string albumName)
        {
            try
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

                    var mapping = foobar.album.First();

                    // add db search for this 
                    var toAdd = new ViewAlbum
                    {
                        Name = mapping.strAlbum,
                        Artist = mapping.strArtist,
                        ReleaseYear = mapping.intYearReleased,
                        Label = mapping.strLabel,
                        Score = mapping.intScore,
                        AlbumThumb = mapping.strAlbumThumb,
                        AlbumThumbBack = mapping.strAlbumThumbBack,
                        Description = mapping.strDescriptionEN,
                        Genre = mapping.strGenre,
                        Mood = mapping.strMood,
                        Review = mapping.strReview,
                        Style = mapping.strStyle,
                        SavedToCollection = true
                    };

                    await _repository.AddAlbums(toAdd);
                }
                    return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> DeleteAlbum(int albumId)
        {
            await _repository.DeleteAlbum(albumId);
            return RedirectToAction("Index");
        }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
