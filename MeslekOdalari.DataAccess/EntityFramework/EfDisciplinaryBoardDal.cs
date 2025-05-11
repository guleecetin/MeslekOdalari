using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.DataAccess.Context;
using MeslekOdalari.DataAccess.Repositories;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.DataAccess.EntityFramework
{
    public class EfDisciplinaryBoardDal : GenericRepository<DisciplinaryBoard>, IDisciplinaryBoardDal
    {
        public EfDisciplinaryBoardDal(MeslekOdalariContext context) : base(context)
        {
        }
    }
}
