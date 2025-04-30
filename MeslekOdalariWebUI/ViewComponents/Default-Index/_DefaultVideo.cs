using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.VideoDtos;
using Microsoft.AspNetCore.Mvc;

namespace MeslekOdalariWebUI.ViewComponents.Default_Index
{
    public class _DefaultVideo:ViewComponent
    {
        private readonly IVideoService _videoService;
        private readonly IMapper _mapper;

        public _DefaultVideo(IMapper mapper, IVideoService videoService)
        {
            _mapper = mapper;
            _videoService = videoService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _videoService.TGetListAsync();
            var videoList = _mapper.Map<List<ResultVideoDto>>(values);
            return View(videoList);
        }
    }
}
