using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Business.Concrete;
using MeslekOdalari.DataAccess.Abstract;
using MeslekOdalari.DataAccess.EntityFramework;
using MeslekOdalari.DataAccess.Repositories;

namespace MeslekOdalariWebUI.Extensions
{
    public static class ServiceExtensions
    {
      
        public static void AddServiceExtensions(this IServiceCollection Services)
        {
            Services.AddScoped<IBannerDal, EfBannerDal>();
            Services.AddScoped<IBannerService, BannerManager>();

            Services.AddScoped<IContactDal, EfContactDal>();
            Services.AddScoped<IContactService, ContactManager>();

            Services.AddScoped<ICounterDal, EfCounterDal>();
            Services.AddScoped<ICounterService, CounterManager>();

            Services.AddScoped<IFeatureDal, EfFeatureDal>();
            Services.AddScoped<IFeatureService, FeatureManager>();

            Services.AddScoped<IMessageDal, EfMessageDal>();
            Services.AddScoped<IMessageService, MessageManager>();

            Services.AddScoped<INewsDal, EfNewsDal>();
            Services.AddScoped<INewsService, NewsManager>();

            Services.AddScoped<IQuestDal, EfQuestDal>();
            Services.AddScoped<IQuestService, QuestManager>();

            Services.AddScoped<IRoomsDal, EfRoomsDal>();
            Services.AddScoped<IRoomsService, RoomsManager>();

            Services.AddScoped<IVideoDal, EfVideoDal>();
            Services.AddScoped<IVideoService, VideoManager>();

            Services.AddScoped<ISubHeaderDal, EfSubHeaderDal>();
            Services.AddScoped<ISubHeaderService, SubHeaderManager>();

            Services.AddScoped<IProjectDal, EfProjectDal>();
            Services.AddScoped<IProjectService, ProjectManager>();

            Services.AddScoped<IRegulationDal, EfRegulationDal>();
            Services.AddScoped<IRegulationService, RegulationManager>();


            Services.AddScoped(typeof(IGenericDal<>), typeof(GenericRepository<>));
            Services.AddScoped(typeof(IGenericService<>), typeof(GenericManager<>));

        }
    }
}
