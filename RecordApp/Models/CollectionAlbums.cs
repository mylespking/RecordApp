using System;

namespace RecordApp.Models
{
    public class CollectionAlbums
    {
        public int AlbumId { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public int Tracks { get; set; }
        public string AlbumType { get; set; }
        public string ReleaseDate { get; set; }
    }
}
