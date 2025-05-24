using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;

namespace MeslekOdalari.Entity.Entities
{
    public class UserLogin:IdentityUserLogin<ObjectId>
    {
    }
}
