using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.RegulationDtos;
using MeslekOdalari.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace MeslekOdalariWebUI.Controllers
{
    public class RegulationController : Controller
    {
        private readonly IRegulationService _regulationService;
        private readonly IMapper _mapper;

        public RegulationController(IMapper mapper, IRegulationService regulationService)
        {
            _mapper = mapper;
            _regulationService = regulationService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _regulationService.TGetListAsync();
            var regulationList = _mapper.Map<List<ResultRegulationDto>>(values);
            return View(regulationList);
        }
        public async Task<IActionResult> DeleteRegulation(ObjectId id)
        {
            await _regulationService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }
        public IActionResult CreateRegulation()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRegulation(CreateRegulationDto createRegulationDto)
        {
            var newRegulation = _mapper.Map<Regulation>(createRegulationDto);
            await _regulationService.TCreateAsync(newRegulation);
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> UpdateRegulation(ObjectId id)
        {
            var value = await _regulationService.TGetByIdAsync(id);
            var updateRegulation = _mapper.Map<UpdateRegulationDto>(value);
            return View(updateRegulation);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateRegulation(UpdateRegulationDto updateRegulationDto)
        {
            var regulation = _mapper.Map<Regulation>(updateRegulationDto);
            await _regulationService.TUpdateAsync(regulation);
            return RedirectToAction("Index");
        }
    }
}
