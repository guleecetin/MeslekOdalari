using MeslekOdalari.Business.Abstract;
using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.Business.Concrete
{
    public class DateManager : GenericManager<Date>, IDateService
    {
        public DateManager(IGenericDal<Date> genericDal) : base(genericDal)
        {
        }
    }
}
