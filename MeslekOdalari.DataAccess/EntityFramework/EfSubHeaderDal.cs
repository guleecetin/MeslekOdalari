using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.DataAccess.Context;
using MeslekOdalari.DataAccess.Repositories;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.DataAccess.EntityFramework
{
    public class EfSubHeaderDal : GenericRepository<SubHeader>, ISubHeaderDal
    {
        public EfSubHeaderDal(MeslekOdalariContext context) : base(context)
        {
        }
    }
}
