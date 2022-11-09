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

        // Return Collection Index page showing the users current collection using the repository to get the data
        public IActionResult Index()
        {
            var albums = _repository.GetAlbums();
            return View(albums);
        }

        // Method to go to the Add view page where users can search for an album
        public async Task<ActionResult> Add(string? query)
        {
            // If a search query has been entered then use the spotify API to search for albums matching the query
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
                    var deserializedData = JsonConvert.DeserializeObject<SearchResponse>(body);

                    // Add the query back to the data set so the back buttons can go back to the search page
                    deserializedData.searchQuery = query;

                    return View(deserializedData);
                }
            }

            // If no search query is input then the view will open ready to search for an album
            return View();
        }

        // Method to show an album with more data such as Genre, Mood and Review if that data is available
        public async Task<ActionResult> ViewAlbum(string? artist, string? albumName, int? albumId, string? searchQuery)
        {
            // First check to see if the album data is in the database so API does not have to be use
            // If an album has an albumID then the data will be stored locally in the database
            if (albumId != null)
            {
                var dbAlbumData = _repository.GetAlbum((int)albumId).First();

                var ownAlbum = new ViewAlbum
                {
                    Name = dbAlbumData.Name,
                    Artist = dbAlbumData.Artist,
                    ReleaseYear = dbAlbumData.ReleaseYear,
                    Label  = dbAlbumData.Label,
                    Score = dbAlbumData.Score,
                    AlbumThumb = dbAlbumData.AlbumThumb,
                    AlbumThumbBack = dbAlbumData.AlbumThumbBack,
                    // Replace function used to keep source formatting of paragraphing and spacing
                    // !!!!!!!!! Needs more work !!!!!!!!!!!!
                    Description = dbAlbumData.Description.Replace("$./;$*", System.Environment.NewLine),
                    Genre = dbAlbumData.Genre,
                    Mood = dbAlbumData.Mood,
                    Review = dbAlbumData.Review,
                    Style = dbAlbumData.Style,
                    SavedToCollection = dbAlbumData.SavedToCollection
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

            // Use AudioDB API to get Album metadata
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
                var deserializedData = JsonConvert.DeserializeObject<AUdatabasesearch>(body);

                var mapping = deserializedData.album.First();

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

        // Method to add album to database after user selects that it should be in their collection
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
                    var deserializedData = JsonConvert.DeserializeObject<AUdatabasesearch>(body);

                    var mapping = deserializedData.album.First();

                    // add db search for this 
                    var albumToAdd = new ViewAlbum
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

                    await _repository.AddAlbums(albumToAdd);
                }
                    return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // Delete Album from database and collection Method
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
