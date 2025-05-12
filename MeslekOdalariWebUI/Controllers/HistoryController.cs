using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.HistoryDtos;
using MeslekOdalari.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace MeslekOdalariWebUI.Controllers
{
    public class HistoryController : Controller
    {
        private readonly IHistoryService _historyService;
        private readonly IMapper _mapper;

        public HistoryController(IMapper mapper, IHistoryService historyService)
        {
            _mapper = mapper;
            _historyService = historyService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _historyService.TGetListAsync();
            var valueList = _mapper.Map<List<ResultHistoryDto>>(values);
            return View(valueList);
        }
        public async Task<IActionResult> DeleteHistory(ObjectId id)
        {
            await _historyService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }

        public IActionResult CreateHistory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateHistory(CreateHistoryDto createHistoryDto)
        {
            var newValue = _mapper.Map<History>(createHistoryDto);
            await _historyService.TCreateAsync(newValue);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> UpdateHistory(ObjectId id)
        {
            var value = await _historyService.TGetByIdAsync(id);
            var updateValue = _mapper.Map<UpdateHistoryDto>(value);
            return View(updateValue);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateHistory(UpdateHistoryDto updateHistoryDto)
        {
            var updateValue = _mapper.Map<History>(updateHistoryDto);
            await _historyService.TUpdateAsync(updateValue);
            return RedirectToAction("Index");

        }
    }
}
