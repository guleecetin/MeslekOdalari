using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.DataAccess.Context;
using MeslekOdalari.DataAccess.Repositories;
using MeslekOdalari.Entity.Entities;


namespace MeslekOdalari.DataAccess.EntityFramework
{
    public class EfDateDal : GenericRepository<Date>, IDateDal
    {
        public EfDateDal(MeslekOdalariContext context) : base(context)
        {
        }
    }
}
