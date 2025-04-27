using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.NewsDtos;
using MeslekOdalari.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace MeslekOdalariWebUI.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly IMapper _mapper;

        public NewsController(IMapper mapper, INewsService newsService)
        {
            _mapper = mapper;
            _newsService = newsService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _newsService.TGetListAsync();
            var valueList = _mapper.Map<List<ResultNewsDto>>(values);
            return View(valueList);
        }
        public async Task<IActionResult> DeleteNews(ObjectId id)
        {
            await _newsService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }

        public IActionResult CreateNews()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateNews(CreateNewsDto createNewsDto)
        {
            var newValue = _mapper.Map<News>(createNewsDto);
            await _newsService.TCreateAsync(newValue);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> UpdateNews(ObjectId id)
        {
            var value = await _newsService.TGetByIdAsync(id);
            var updateValue = _mapper.Map<UpdateNewsDto>(value);
            return View(updateValue);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateNews(UpdateNewsDto updateNewsDto)
        {
            var updateValue = _mapper.Map<News>(updateNewsDto);
            await _newsService.TUpdateAsync(updateValue);
            return RedirectToAction("Index");

        }
    }
}
