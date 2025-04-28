using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.FeatureDtos;
using MeslekOdalari.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace MeslekOdalariWebUI.Controllers
{
    public class FeatureController : Controller
    {
        private readonly IFeatureService _featureService;
        private readonly IMapper _mapper;

        public FeatureController(IMapper mapper, IFeatureService featureService)
        {
            _mapper = mapper;
            _featureService = featureService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _featureService.TGetListAsync();
            var valueList = _mapper.Map<List<ResultFeatureDto>>(values);
            return View(valueList);
        }
        public async Task<IActionResult> DeleteFeature(ObjectId id)
        {
            await _featureService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }

        public IActionResult CreateFeature()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateFeature(CreateFeatureDto createFeatureDto)
        {
            var newValue = _mapper.Map<Feature>(createFeatureDto);
            await _featureService.TCreateAsync(newValue);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> UpdateFeature(ObjectId id)
        {
            var value = await _featureService.TGetByIdAsync(id);
            var updateValue = _mapper.Map<UpdateFeatureDto>(value);
            return View(updateValue);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateFeature(UpdateFeatureDto updateFeatureDto)
        {
            var updateValue = _mapper.Map<Feature>(updateFeatureDto);
            await _featureService.TUpdateAsync(updateValue);
            return RedirectToAction("Index");

        }
    }
}
