using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.AboutDtos;
using MeslekOdalari.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace MeslekOdalariWebUI.Controllers
{
    public class AboutController : Controller
    {
        private readonly IAboutService _aboutService;
        private readonly IMapper _mapper;

        public AboutController(IMapper mapper, IAboutService aboutService)
        {
            _mapper = mapper;
            _aboutService = aboutService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _aboutService.TGetListAsync();
            var valueList = _mapper.Map<List<ResultAboutDto>>(values);
            return View(valueList);
        }
        public async Task<IActionResult> DeleteAbout(ObjectId id)
        {
            await _aboutService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }

        public IActionResult CreateAbout()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAbout(CreateAboutDto createAboutDto)
        {
            var newValue = _mapper.Map<About>(createAboutDto);
            await _aboutService.TCreateAsync(newValue);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> UpdateAbout(ObjectId id)
        {
            var value = await _aboutService.TGetByIdAsync(id);
            var updateValue = _mapper.Map<UpdateAboutDto>(value);
            return View(updateValue);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAbout(UpdateAboutDto updateAboutDto)
        {
            var updateValue = _mapper.Map<About>(updateAboutDto);
            await _aboutService.TUpdateAsync(updateValue);
            return RedirectToAction("Index");

        }
    }
}
