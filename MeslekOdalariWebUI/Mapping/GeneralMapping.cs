using AutoMapper;
using MeslekOdalari.Dto.Dtos.BannerDtos;
using MeslekOdalari.Entity.Entities;

namespace MeslekOdalariWebUI.Mapping
{
    public class GeneralMapping:Profile
    {
        public GeneralMapping()
        {
            CreateMap<ResultBannerDto, Banner>().ReverseMap();
            CreateMap<UpdateBannerDto, Banner>().ReverseMap();
            CreateMap<CreateBannerDto, Banner>().ReverseMap();
        }
    }
}
