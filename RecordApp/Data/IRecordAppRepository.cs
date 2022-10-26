using System.Collections.Generic;
using RecordApp.Models;
using RecordsApp.Data.Entities;

namespace RecordApp.Data
{
    public interface IRecordAppRepository
    {
        IEnumerable<CollectionAlbums> GetAlbums();
    }
}
