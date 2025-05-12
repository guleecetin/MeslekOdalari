using MeslekOdalari.Business.Abstract;
using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.Business.Concrete
{
    public class AboutManager : GenericManager<About>, IAboutService
    {
        public AboutManager(IGenericDal<About> genericDal) : base(genericDal)
        {
        }
    }
}
