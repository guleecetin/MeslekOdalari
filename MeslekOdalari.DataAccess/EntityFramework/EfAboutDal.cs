using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.DataAccess.Context;
using MeslekOdalari.DataAccess.Repositories;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.DataAccess.EntityFramework
{
    public class EfAboutDal : GenericRepository<About>, IAboutDal
    {
        public EfAboutDal(MeslekOdalariContext context) : base(context)
        {
        }
    }
}
