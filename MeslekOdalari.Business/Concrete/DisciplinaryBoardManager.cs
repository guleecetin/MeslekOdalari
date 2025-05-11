using MeslekOdalari.Business.Abstract;
using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.Business.Concrete
{
    public class DisciplinaryBoardManager : GenericManager<DisciplinaryBoard>, IDisciplinaryBoardService
    {
        public DisciplinaryBoardManager(IGenericDal<DisciplinaryBoard> genericDal) : base(genericDal)
        {
        }
    }
}
