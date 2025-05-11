using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.DisciplinaryBoardDtos;
using Microsoft.AspNetCore.Mvc;

namespace MeslekOdalariWebUI.Controllers
{
    public class UIDisciplinaryBoardController : Controller
    {
        private readonly IDisciplinaryBoardService _disciplinaryBoardService;
        private readonly IMapper _mapper;

        public UIDisciplinaryBoardController(IMapper mapper, IDisciplinaryBoardService disciplinaryBoardService)
        {
            _mapper = mapper;
            _disciplinaryBoardService = disciplinaryBoardService;
        }

        public async Task<IActionResult> DisiplinKurulu()
        {
            var values = await _disciplinaryBoardService.TGetListAsync();
            var valueList = _mapper.Map<List<ResultDisciplinaryBoardDto>>(values);
            return View(valueList);
        }
    }
}
