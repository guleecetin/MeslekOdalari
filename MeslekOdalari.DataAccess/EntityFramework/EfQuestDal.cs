using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.DataAccess.Context;
using MeslekOdalari.DataAccess.Repositories;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.DataAccess.EntityFramework
{
    public class EfQuestDal : GenericRepository<Quest>, IQuestDal
    {
        public EfQuestDal(MeslekOdalariContext context) : base(context)
        {
        }
    }
}
