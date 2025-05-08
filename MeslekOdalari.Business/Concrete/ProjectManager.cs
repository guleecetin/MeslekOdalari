using MeslekOdalari.Business.Abstract;
using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalari.Business.Concrete
{
    public class ProjectManager : GenericManager<Project>, IProjectService
    {
        public ProjectManager(IGenericDal<Project> genericDal) : base(genericDal)
        {
        }
    }
}
