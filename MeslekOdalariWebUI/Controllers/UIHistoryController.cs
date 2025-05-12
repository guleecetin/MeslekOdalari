using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.HistoryDtos;
using Microsoft.AspNetCore.Mvc;

namespace MeslekOdalariWebUI.Controllers
{
    public class UIHistoryController : Controller
    {
        private readonly IHistoryService _historyService;
        private readonly IMapper _mapper;

        public UIHistoryController(IMapper mapper, IHistoryService historyService)
        {
            _mapper = mapper;
            _historyService = historyService;
        }

        public async Task<IActionResult> Tarihcemiz()
        {
            var values = await _historyService.TGetListAsync();
            var valueList = _mapper.Map<List<ResultHistoryDto>>(values);
            return View(valueList);
        }

    }
}
