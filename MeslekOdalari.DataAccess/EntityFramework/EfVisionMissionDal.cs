using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.DataAccess.Context;
using MeslekOdalari.DataAccess.Repositories;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.DataAccess.EntityFramework
{
    public class EfVisionMissionDal : GenericRepository<VisionMission>, IVisionMissionDal
    {
        public EfVisionMissionDal(MeslekOdalariContext context) : base(context)
        {
        }
    }
}
