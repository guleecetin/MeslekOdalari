using MeslekOdalari.Business.Abstract;
using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.Business.Concrete
{
    public class RegulationManager : GenericManager<Regulation>, IRegulationService
    {
        public RegulationManager(IGenericDal<Regulation> genericDal) : base(genericDal)
        {
        }
    }
}
