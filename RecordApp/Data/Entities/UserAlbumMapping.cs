using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordApp.Data.Entities
{
    public class UserAlbumMapping
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AlbumId { get; set; }
    }
}
