using MeslekOdalari.Business.Abstract;
using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.Business.Concrete
{
    public class ContactManager : GenericManager<Contact>, IContactService
    {
        public ContactManager(IGenericDal<Contact> genericDal) : base(genericDal)
        {
        }
    }
}
