using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.AboutDtos;
using MeslekOdalari.Dto.Dtos.VisionMissionDtos;
using MeslekOdalari.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace MeslekOdalariWebUI.Controllers
{
    public class VisionMissionController : Controller
    {
        private readonly IVisionMissionService _visionMissionService;
        private readonly IMapper _mapper;

        public VisionMissionController(IMapper mapper, IVisionMissionService visionMissionService)
        {
            _mapper = mapper;
            _visionMissionService = visionMissionService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _visionMissionService.TGetListAsync();
            var valueList = _mapper.Map<List<ResultVisionMissionDto>>(values);
            return View(valueList);
        }
        public async Task<IActionResult> DeleteVisionMission(ObjectId id)
        {
            await _visionMissionService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }

        public IActionResult CreateVisionMission()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateVisionMission(CreateVisionMissionDto createVisionMissionDto)
        {
            var newValue = _mapper.Map<VisionMission>(createVisionMissionDto);
            await _visionMissionService.TCreateAsync(newValue);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> UpdateVisionMission(ObjectId id)
        {
            var value = await _visionMissionService.TGetByIdAsync(id);
            var updateValue = _mapper.Map<UpdateVisionMissionDto>(value);
            return View(updateValue);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateVisionMission(UpdateVisionMissionDto updateVisionMissionDto)
        {
            var updateValue = _mapper.Map<VisionMission>(updateVisionMissionDto);
            await _visionMissionService.TUpdateAsync(updateValue);
            return RedirectToAction("Index");

        }
    }
}
