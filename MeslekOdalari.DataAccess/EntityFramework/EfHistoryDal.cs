using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.DataAccess.Context;
using MeslekOdalari.DataAccess.Repositories;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.DataAccess.EntityFramework
{
    public class EfHistoryDal : GenericRepository<History>, IHistoryDal
    {
        public EfHistoryDal(MeslekOdalariContext context) : base(context)
        {
        }
    }
}
