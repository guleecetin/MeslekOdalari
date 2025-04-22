using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.DataAccess.Context;
using MeslekOdalari.DataAccess.Repositories;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.DataAccess.EntityFramework
{
    public class EfNewsDal : GenericRepository<News>, INewsDal
    {
        public EfNewsDal(MeslekOdalariContext context) : base(context)
        {
        }
    }
}
