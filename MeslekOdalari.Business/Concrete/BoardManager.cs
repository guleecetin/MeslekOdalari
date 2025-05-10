using MeslekOdalari.Business.Abstract;
using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.Business.Concrete
{
    public class BoardManager : GenericManager<Board>, IBoardService
    {
        public BoardManager(IGenericDal<Board> genericDal) : base(genericDal)
        {
        }
    }
}
