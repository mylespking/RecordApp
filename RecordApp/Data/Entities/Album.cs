using System;

namespace RecordsApp.Data.Entities
{
  public class Album
  {
    public int AlbumId { get; set; }
    public string Name { get; set; }
    public string Artist { get; set; }
    public int Tracks { get; set; }
    public string AlbumType { get; set; }
    public DateTime ReleaseDate { get; set; }
  }
}
