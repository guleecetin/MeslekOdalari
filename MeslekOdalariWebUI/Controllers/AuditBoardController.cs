using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.AuditBoardDtos;
using MeslekOdalari.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace MeslekOdalariWebUI.Controllers
{
    public class AuditBoardController : Controller
    {
        private readonly IAuditBoardService _auditBoardService;
        private readonly IMapper _mapper;

        public AuditBoardController(IMapper mapper, IAuditBoardService auditBoardService)
        {
            _mapper = mapper;
            _auditBoardService = auditBoardService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _auditBoardService.TGetListAsync();
            var valueList = _mapper.Map<List<ResultAuditBoardDto>>(values);
            return View(valueList);
        }
        public async Task<IActionResult> DeleteAuditBoard(ObjectId id)
        {
            await _auditBoardService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }

        public IActionResult CreateAuditBoard()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAuditBoard(CreateAuditBoardDto createAuditBoardDto)
        {
            var newValue = _mapper.Map<AuditBoard>(createAuditBoardDto);
            await _auditBoardService.TCreateAsync(newValue);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> UpdateAuditBoard(ObjectId id)
        {
            var value = await _auditBoardService.TGetByIdAsync(id);
            var updateValue = _mapper.Map<UpdateAuditBoardDto>(value);
            return View(updateValue);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAuditBoard(UpdateAuditBoardDto updateAuditBoardDto)
        {
            var updateValue = _mapper.Map<AuditBoard>(updateAuditBoardDto);
            await _auditBoardService.TUpdateAsync(updateValue);
            return RedirectToAction("Index");

        }
    }
}
