using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.DisciplinaryBoardDtos;
using MeslekOdalari.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace MeslekOdalariWebUI.Controllers
{
    public class DisciplinaryBoardController : Controller
    {
        private readonly IDisciplinaryBoardService _disciplinaryBoardService;
        private readonly IMapper _mapper;

        public DisciplinaryBoardController(IMapper mapper, IDisciplinaryBoardService disciplinaryBoardService)
        {
            _mapper = mapper;
            _disciplinaryBoardService = disciplinaryBoardService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _disciplinaryBoardService.TGetListAsync();
            var valueList = _mapper.Map<List<ResultDisciplinaryBoardDto>>(values);
            return View(valueList);
        }
        public async Task<IActionResult> DeleteDisciplinaryBoard(ObjectId id)
        {
            await _disciplinaryBoardService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }

        public IActionResult CreateDisciplinaryBoard()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateDisciplinaryBoard(CreateDisciplinaryBoardDto createDisciplinaryBoardDto)
        {
            var newValue = _mapper.Map<DisciplinaryBoard>(createDisciplinaryBoardDto);
            await _disciplinaryBoardService.TCreateAsync(newValue);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> UpdateDisciplinaryBoard(ObjectId id)
        {
            var value = await _disciplinaryBoardService.TGetByIdAsync(id);
            var updateValue = _mapper.Map<UpdateDisciplinaryBoardDto>(value);
            return View(updateValue);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateDisciplinaryBoard(UpdateDisciplinaryBoardDto updateDisciplinaryBoardDto)
        {
            var updateValue = _mapper.Map<DisciplinaryBoard>(updateDisciplinaryBoardDto);
            await _disciplinaryBoardService.TUpdateAsync(updateValue);
            return RedirectToAction("Index");

        }
    }
}
