using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.DataAccess.Context;
using MeslekOdalari.DataAccess.Repositories;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.DataAccess.EntityFramework
{
    public class EfRegulationDal : GenericRepository<Regulation>, IRegulationDal
    {
        public EfRegulationDal(MeslekOdalariContext context) : base(context)
        {
        }
    }
}
