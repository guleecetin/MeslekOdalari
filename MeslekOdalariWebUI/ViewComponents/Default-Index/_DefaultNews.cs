using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.NewsDtos;
using Microsoft.AspNetCore.Mvc;

namespace MeslekOdalariWebUI.ViewComponents.Default_Index
{
    public class _DefaultNews:ViewComponent
    {
        private readonly INewsService _newsService;
        private readonly IMapper _mapper;

        public _DefaultNews(IMapper mapper, INewsService newsService)
        {
            _mapper = mapper;
            _newsService = newsService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _newsService.TGetListAsync();
            var newsList = _mapper.Map<List<ResultNewsDto>>(values);
            return View(newsList);
        }
    }
}
