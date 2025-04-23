using MeslekOdalari.Business.Abstract;
using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.Business.Concrete
{
    public class QuestManager : GenericManager<Quest>, IQuestService
    {
        public QuestManager(IGenericDal<Quest> genericDal) : base(genericDal)
        {
        }
    }
}
