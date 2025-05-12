using MeslekOdalari.Business.Abstract;
using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.Business.Concrete
{
    public class HistoryManager : GenericManager<History>, IHistoryService
    {
        public HistoryManager(IGenericDal<History> genericDal) : base(genericDal)
        {
        }
    }
}
