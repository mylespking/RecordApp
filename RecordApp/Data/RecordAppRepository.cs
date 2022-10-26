using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<CollectionAlbums> GetAlbums()
        {
            var albums = _ctx.Album.ToList();

            var dataresults = new List<CollectionAlbums>();

            foreach (var a in albums)
            {
                var Data = new CollectionAlbums
                {
                    AlbumId = a.AlbumId,
                    Name = a.Name,
                    Artist = a.Artist,
                    Tracks = a.Tracks,
                    AlbumType = a.AlbumType,
                    ReleaseDate = a.ReleaseDate.ToString("dd MMMM yyyy")
                };

                dataresults.Add(Data);
            }

            return dataresults;
        }
    }
}
