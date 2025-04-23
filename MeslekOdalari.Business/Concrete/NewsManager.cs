using MeslekOdalari.Business.Abstract;
using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.Business.Concrete
{
    public class NewsManager : GenericManager<News>, INewsService
    {
        public NewsManager(IGenericDal<News> genericDal) : base(genericDal)
        {
        }
    }
}
