using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.DataAccess.Context;
using MeslekOdalari.DataAccess.Repositories;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.DataAccess.EntityFramework
{
    public class EfAuditBoardDal : GenericRepository<AuditBoard>, IAuditBoardDal
    {
        public EfAuditBoardDal(MeslekOdalariContext context) : base(context)
        {
        }
    }
}
