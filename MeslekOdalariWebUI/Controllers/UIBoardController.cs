using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.BoardDtos;
using Microsoft.AspNetCore.Mvc;

namespace MeslekOdalariWebUI.Controllers
{
    public class UIBoardController : Controller
    {
        private readonly IBoardService _boardService;
        private readonly IMapper _mapper;

        public UIBoardController(IMapper mapper, IBoardService boardService)
        {
            _mapper = mapper;
            _boardService = boardService;
        }

        public async Task<IActionResult> YonetimKurulu()
        {
            var values = await _boardService.TGetListAsync();
            var valueList = _mapper.Map<List<ResultBoardDto>>(values);
            return View(valueList);
        }


        /*


public async Task<IActionResult> DisiplinKurulu()
{
    var values = await _boardService.TGetListAsync();
    var valueList = _mapper.Map<List<ResultBoardDto>>(values);
    return View(valueList);
}
*/

    }
}
