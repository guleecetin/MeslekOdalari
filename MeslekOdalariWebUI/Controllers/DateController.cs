using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.DateDtos;
using MeslekOdalari.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace MeslekOdalariWebUI.Controllers
{
    public class DateController : Controller
    {
        private readonly IDateService _dateService;
        private readonly IMapper _mapper;

        public DateController(IMapper mapper, IDateService dateService)
        {
            _mapper = mapper;
            _dateService = dateService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _dateService.TGetListAsync();
            var valueList = _mapper.Map<List<ResultDateDto>>(values);
            return View(valueList);
        }
        public async Task<IActionResult> DeleteDate(ObjectId id)
        {
            await _dateService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }

        public IActionResult CreateDate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateDate(CreateDateDto createDateDto)
        {
            var newValue = _mapper.Map<Date>(createDateDto);
            await _dateService.TCreateAsync(newValue);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> UpdateDate(ObjectId id)
        {
            var value = await _dateService.TGetByIdAsync(id);
            var updateValue = _mapper.Map<UpdateDateDto>(value);
            return View(updateValue);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateDate(UpdateDateDto updateDateDto)
        {
            var updateValue = _mapper.Map<Date>(updateDateDto);
            await _dateService.TUpdateAsync(updateValue);
            return RedirectToAction("Index");

        }
    }
}
