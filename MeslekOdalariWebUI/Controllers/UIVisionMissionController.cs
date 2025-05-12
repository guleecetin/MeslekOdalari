using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.VisionMissionDtos;
using Microsoft.AspNetCore.Mvc;

namespace MeslekOdalariWebUI.Controllers
{
    public class UIVisionMissionController : Controller
    {
        private readonly IVisionMissionService _visionMissionService;
        private readonly IMapper _mapper;

        public UIVisionMissionController(IMapper mapper, IVisionMissionService visionMissionService)
        {
            _mapper = mapper;
            _visionMissionService = visionMissionService;
        }

        public async Task<IActionResult> VizyonMisyon()
        {
            var values = await _visionMissionService.TGetListAsync();
            var valueList = _mapper.Map<List<ResultVisionMissionDto>>(values);
            return View(valueList);
        }
    }
}
