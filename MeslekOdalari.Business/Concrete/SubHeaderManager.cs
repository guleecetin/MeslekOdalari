using MeslekOdalari.Business.Abstract;
using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.Business.Concrete
{
    public class SubHeaderManager : GenericManager<SubHeader>, ISubHeaderService
    {
        public SubHeaderManager(IGenericDal<SubHeader> genericDal) : base(genericDal)
        {
        }
    }
}
