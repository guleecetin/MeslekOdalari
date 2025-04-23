using MeslekOdalari.Business.Abstract;
using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.Business.Concrete
{
    public class CounterManager : GenericManager<Counter>, ICounterService
    {
        public CounterManager(IGenericDal<Counter> genericDal) : base(genericDal)
        {
        }
    }
}
