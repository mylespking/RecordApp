using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecordApp.Models;
using RecordsApp.Data.Entities;

namespace RecordApp.Data
{
    public class RecordAppRepository : IRecordAppRepository
    {
        private readonly RecordAppContext _ctx;
        private readonly ILogger<RecordAppRepository> _logger;

        public RecordAppRepository(RecordAppContext ctx, ILogger<RecordAppRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        // Method to retrieve all albums from the local database
        public IEnumerable<ViewAlbum> GetAlbums()
        {
            var albumData = _ctx.Album.ToList();

            var dataresults = new List<ViewAlbum>();

            foreach (var a in albumData)
            {
                var Data = new ViewAlbum
                {
                    AlbumId = a.AlbumId,
                    Name = a.Name,
                    Artist = a.Artist,
                    ReleaseYear = a.ReleaseYear,
                    Label = a.Label,
                    Score = a.Score,
                    AlbumThumb = a.AlbumThumb,
                    AlbumThumbBack = a.AlbumThumbBack,
                    Description = a.Description,
                    Genre = a.Genre,
                    Mood = a.Mood,
                    Review = a.Review,
                    Style = a.Style,
                    SavedToCollection = true
                };

                dataresults.Add(Data);
            }

            return dataresults;
        }

        // Method to retrieve single album from database
        public IEnumerable<ViewAlbum> GetAlbum(int albumId)
        {
            IEnumerable<ViewAlbum> albumData =
                _ctx.Album.Where(x => x.AlbumId == albumId)
                .Select(x =>
                    new ViewAlbum
                    {
                        AlbumId = x.AlbumId,
                        Name = x.Name,
                        Artist = x.Artist,
                        ReleaseYear = x.ReleaseYear,
                        Label = x.Label,
                        Score = x.Score,
                        AlbumThumb = x.AlbumThumb,
                        AlbumThumbBack = x.AlbumThumbBack,
                        Description = x.Description,
                        Genre = x.Genre,
                        Mood = x.Mood,
                        Review = x.Review,
                        Style = x.Style,
                        SavedToCollection = true
                    }).ToList();

            return albumData;
        }

        // Methoid to add album to the album table
        public async Task<bool> AddAlbums(ViewAlbum model)
        {
            var dataToAdd = new Album
            {
                Name = model.Name,
                Artist = model.Artist,
                ReleaseYear = model.ReleaseYear,
                Label = model.Label,
                Score = model.Score,
                AlbumThumb = model.AlbumThumb,
                AlbumThumbBack = model.AlbumThumbBack,
                // Replace function used to keep source formatting of paragraphing and spacing
                // !!!!!!!!! Needs more work !!!!!!!!!!!!
                Description = model.Description.Replace("\n", "$./;$*"),
                Genre = model.Genre,
                Mood = model.Mood,
                Review = model.Review,
                Style = model.Style,
                SavedToCollection = true
            };

            try
            {
                _ctx.Album.Add(dataToAdd);
                await _ctx.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        // Method to delete album from the table 
        public async Task<bool> DeleteAlbum (int albumId)
        {
            try
            {
                var toDelete = new Album { AlbumId = albumId };

                _ctx.Album.Attach(toDelete);
                _ctx.Album.Remove(toDelete);
                await _ctx.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
