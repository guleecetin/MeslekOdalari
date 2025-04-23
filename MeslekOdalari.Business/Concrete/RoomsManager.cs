using MeslekOdalari.Business.Abstract;
using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.Business.Concrete
{
    public class RoomsManager : GenericManager<Rooms>, IRoomsService
    {
        public RoomsManager(IGenericDal<Rooms> genericDal) : base(genericDal)
        {
        }
    }
}
