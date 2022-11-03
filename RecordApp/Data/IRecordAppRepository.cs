using System.Collections.Generic;
using System.Threading.Tasks;
using RecordApp.Models;
using RecordsApp.Data.Entities;

namespace RecordApp.Data
{
    public interface IRecordAppRepository
    {
        IEnumerable<ViewAlbum> GetAlbums();
        IEnumerable<ViewAlbum> GetAlbum(int AlbumId);
        Task<bool> AddAlbums(ViewAlbum model);
    }
}
