using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;

namespace MeslekOdalari.Entity.Entities
{
    public class AppUser:IdentityUser<ObjectId>
    {
        public string NameSurName { get; set; }
    }
}
