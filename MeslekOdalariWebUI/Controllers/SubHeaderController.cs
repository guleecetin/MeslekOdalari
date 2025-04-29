using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.SubHeaderDtos;
using MeslekOdalari.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace MeslekOdalariWebUI.Controllers
{
    public class SubHeaderController : Controller
    {
        private readonly ISubHeaderService _subHeaderService;
        private readonly IMapper _mapper;

        public SubHeaderController(IMapper mapper, ISubHeaderService subHeaderService)
        {
            _mapper = mapper;
            _subHeaderService = subHeaderService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _subHeaderService.TGetListAsync();
            var valueList = _mapper.Map<List<ResultSubHeaderDto>>(values);
            return View(valueList);
        }
        public async Task<IActionResult> DeleteSubHeader(ObjectId id)
        {
            await _subHeaderService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }

        public IActionResult CreateSubHeader()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateSubHeader(CreateSubHeaderDto createSubHeaderDto)
        {
            var newValue = _mapper.Map<SubHeader>(createSubHeaderDto);
            await _subHeaderService.TCreateAsync(newValue);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> UpdateSubHeader(ObjectId id)
        {
            var value = await _subHeaderService.TGetByIdAsync(id);
            var updateValue = _mapper.Map<UpdateSubHeaderDto>(value);
            return View(updateValue);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSubHeader(UpdateSubHeaderDto updateSubHeaderDto)
        {
            var updateValue = _mapper.Map<SubHeader>(updateSubHeaderDto);
            await _subHeaderService.TUpdateAsync(updateValue);
            return RedirectToAction("Index");

        }
    }
}
