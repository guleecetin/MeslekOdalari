using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.BoardDtos;
using MeslekOdalari.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace MeslekOdalariWebUI.Controllers
{
    public class BoardController : Controller
    {
        private readonly IBoardService _boardService;
        private readonly IMapper _mapper;

        public BoardController(IMapper mapper, IBoardService boardService)
        {
            _mapper = mapper;
            _boardService = boardService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _boardService.TGetListAsync();
            var valueList = _mapper.Map<List<ResultBoardDto>>(values);
            return View(valueList);
        }
        public async Task<IActionResult> DeleteBoard(ObjectId id)
        {
            await _boardService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }

        public IActionResult CreateBoard()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateBoard(CreateBoardDto createBoardDto)
        {
            var newValue = _mapper.Map<Board>(createBoardDto);
            await _boardService.TCreateAsync(newValue);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> UpdateBoard(ObjectId id)
        {
            var value = await _boardService.TGetByIdAsync(id);
            var updateValue = _mapper.Map<UpdateBoardDto>(value);
            return View(updateValue);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBoard(UpdateBoardDto updateBoardDto)
        {
            var updateValue = _mapper.Map<Board>(updateBoardDto);
            await _boardService.TUpdateAsync(updateValue);
            return RedirectToAction("Index");

        }
    }
}
