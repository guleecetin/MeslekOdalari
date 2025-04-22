using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.DataAccess.Context;
using MeslekOdalari.DataAccess.Repositories;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.DataAccess.EntityFramework
{
    public class EfRoomsDal : GenericRepository<Rooms>, IRoomsDal
    {
        public EfRoomsDal(MeslekOdalariContext context) : base(context)
        {
        }
    }
}
