using MeslekOdalari.Business.Abstract;
using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.Business.Concrete
{
    public class FeatureManager : GenericManager<Feature>, IFeatureService
    {
        public FeatureManager(IGenericDal<Feature> genericDal) : base(genericDal)
        {
        }
    }
}
