using MeslekOdalari.Business.Abstract;
using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.Business.Concrete
{
    public class BannerManager : GenericManager<Banner>, IBannerService
    {
        public BannerManager(IGenericDal<Banner> genericDal) : base(genericDal)
        {
        }
    }
}
