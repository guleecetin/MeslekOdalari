using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.AuditBoardDtos;
using Microsoft.AspNetCore.Mvc;

namespace MeslekOdalariWebUI.Controllers
{
    public class UIAuditBoardController : Controller
    {
        private readonly IAuditBoardService _auditBoardService;
        private readonly IMapper _mapper;

        public UIAuditBoardController(IMapper mapper, IAuditBoardService auditBoardService)
        {
            _mapper = mapper;
            _auditBoardService = auditBoardService;
        }

        public async Task<IActionResult> DenetimKurulu()
        {
            var values = await _auditBoardService.TGetListAsync();
            var valueList = _mapper.Map<List<ResultAuditBoardDto>>(values);
            return View(valueList);
        }
    }
}
