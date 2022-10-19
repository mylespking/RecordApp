using System.Collections.Generic;
using RecordsApp.Data.Entities;

namespace RecordApp.Data.Entities
{
  public class UserDetail
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
  }
}