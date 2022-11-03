using System;

namespace RecordsApp.Data.Entities
{
  public class Album
  {
    public int AlbumId { get; set; }
    public string Name { get; set; }
    public string Artist { get; set; }
    public string ReleaseYear { get; set; }
    public string Label { get; set; }
    public string Score { get; set; }
    public string AlbumThumb { get; set; }
    public string AlbumThumbBack { get; set; }
    public string Description { get; set; }
    public string Genre { get; set; }
    public string Mood { get; set; }
    public string Review { get; set; }
    public string Style { get; set; }
    public bool SavedToCollection { get; set; }
   }
}
