using MeslekOdalari.Business.Abstract;
using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.Business.Concrete
{
    public class AuditBoardManager : GenericManager<AuditBoard>, IAuditBoardService
    {
        public AuditBoardManager(IGenericDal<AuditBoard> genericDal) : base(genericDal)
        {
        }
    }
}
