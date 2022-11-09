using System.Collections.Generic;
using System.Threading.Tasks;
using RecordApp.Models;
using RecordsApp.Data.Entities;

namespace RecordApp.Data
{
    public interface IRecordAppRepository
    {
        IEnumerable<ViewAlbum> GetAlbums();
        IEnumerable<ViewAlbum> GetAlbum(int albumId);
        Task<bool> AddAlbums(ViewAlbum model);
        Task<bool> DeleteAlbum(int albumId);
    }
}
