using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;

namespace MeslekOdalari.Entity.Entities
{
    public class AppRole:IdentityRole<ObjectId>
    {
    }
}
