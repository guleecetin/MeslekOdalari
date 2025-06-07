using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;

namespace MeslekOdalari.Entity.Entities
{
    public class AppRole:IdentityRole<ObjectId>
    {
        public AppRole() : base()
        {
            Id = ObjectId.GenerateNewId();
        }

        public AppRole(string roleName) : base(roleName)
        {
            Id = ObjectId.GenerateNewId();
        }
    }
}