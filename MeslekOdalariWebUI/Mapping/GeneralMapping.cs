using AutoMapper;
using MeslekOdalari.Dto.Dtos.AboutDtos;
using MeslekOdalari.Dto.Dtos.AuditBoardDtos;
using MeslekOdalari.Dto.Dtos.BannerDtos;
using MeslekOdalari.Dto.Dtos.BoardDtos;
using MeslekOdalari.Dto.Dtos.ContactDtos;
using MeslekOdalari.Dto.Dtos.CounterDtos;
using MeslekOdalari.Dto.Dtos.DisciplinaryBoardDtos;
using MeslekOdalari.Dto.Dtos.FeatureDtos;
using MeslekOdalari.Dto.Dtos.MessageDtos;
using MeslekOdalari.Dto.Dtos.NewsDtos;
using MeslekOdalari.Dto.Dtos.ProjectDtos;
using MeslekOdalari.Dto.Dtos.QuestDtos;
using MeslekOdalari.Dto.Dtos.RegulationDtos;
using MeslekOdalari.Dto.Dtos.RoomsDto;
using MeslekOdalari.Dto.Dtos.SubHeaderDtos;
using MeslekOdalari.Dto.Dtos.VideoDtos;
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

            CreateMap<ResultContactDto, Contact>().ReverseMap();
            CreateMap<UpdateContactDto, Contact>().ReverseMap();
            CreateMap<CreateContactDto, Contact>().ReverseMap();

            CreateMap<ResultCounterDto, Counter>().ReverseMap();
            CreateMap<UpdateCounterDto, Counter>().ReverseMap();
            CreateMap<CreateCounterDto, Counter>().ReverseMap();

            CreateMap<ResultFeatureDto, Feature>().ReverseMap();
            CreateMap<UpdateFeatureDto, Feature>().ReverseMap();
            CreateMap<CreateFeatureDto, Feature>().ReverseMap();

            CreateMap<ResultMessageDto, Message>().ReverseMap();
            CreateMap<UpdateMessageDto, Message>().ReverseMap();
            CreateMap<CreateMessageDto, Message>().ReverseMap();

            CreateMap<ResultNewsDto, News>().ReverseMap();
            CreateMap<UpdateNewsDto, News>().ReverseMap();
            CreateMap<CreateNewsDto, News>().ReverseMap();

            CreateMap<ResultQuestDto, Quest>().ReverseMap();
            CreateMap<UpdateQuestDto, Quest>().ReverseMap();
            CreateMap<CreateQuestDto, Quest>().ReverseMap();

            CreateMap<ResultRoomsDto, Rooms>().ReverseMap();
            CreateMap<UpdateRoomsDto, Rooms>().ReverseMap();
            CreateMap<CreateRoomsDto, Rooms>().ReverseMap();

            CreateMap<ResultVideoDto, Video>().ReverseMap();
            CreateMap<UpdateVideoDto, Video>().ReverseMap();
            CreateMap<CreateVideoDto, Video>().ReverseMap();

            CreateMap<ResultSubHeaderDto, SubHeader>().ReverseMap();
            CreateMap<UpdateSubHeaderDto, SubHeader>().ReverseMap();
            CreateMap<CreateSubHeaderDto, SubHeader>().ReverseMap();

            CreateMap<ResultProjectDto, Project>().ReverseMap();
            CreateMap<UpdateProjectDto, Project>().ReverseMap();
            CreateMap<CreateProjectDto, Project>().ReverseMap();

            CreateMap<ResultRegulationDto, Regulation>().ReverseMap();
            CreateMap<UpdateRegulationDto, Regulation>().ReverseMap();
            CreateMap<CreateRegulationDto, Regulation>().ReverseMap();

            CreateMap<ResultBoardDto, Board>().ReverseMap();
            CreateMap<UpdateBoardDto, Board>().ReverseMap();
            CreateMap<CreateBoardDto, Board>().ReverseMap();

            CreateMap<ResultAuditBoardDto, AuditBoard>().ReverseMap();
            CreateMap<UpdateAuditBoardDto, AuditBoard>().ReverseMap();
            CreateMap<CreateAuditBoardDto, AuditBoard>().ReverseMap();

            CreateMap<ResultDisciplinaryBoardDto, DisciplinaryBoard>().ReverseMap();
            CreateMap<UpdateDisciplinaryBoardDto, DisciplinaryBoard>().ReverseMap();
            CreateMap<CreateDisciplinaryBoardDto, DisciplinaryBoard>().ReverseMap();

            CreateMap<ResultAboutDto, About>().ReverseMap();
            CreateMap<UpdateAboutDto, About>().ReverseMap();
            CreateMap<CreateAboutDto, About>().ReverseMap();


        }
    }
}
