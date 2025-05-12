using MeslekOdalari.Business.Abstract;
using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.Business.Concrete
{
    public class VisionMissionManager : GenericManager<VisionMission>, IVisionMissionService
    {
        public VisionMissionManager(IGenericDal<VisionMission> genericDal) : base(genericDal)
        {
        }
    }
}
