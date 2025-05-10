using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.DataAccess.Context;
using MeslekOdalari.DataAccess.Repositories;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.DataAccess.EntityFramework
{
    public class EfBoardDal : GenericRepository<Board>, IBoardDal
    {
        public EfBoardDal(MeslekOdalariContext context) : base(context)
        {
        }
    }
}
