using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.AboutDtos;
using Microsoft.AspNetCore.Mvc;

namespace MeslekOdalariWebUI.Controllers
{
    public class UIAboutController : Controller
    {
        private readonly IAboutService _aboutService;
        private readonly IMapper _mapper;

        public UIAboutController(IMapper mapper, IAboutService aboutService)
        {
            _mapper = mapper;
            _aboutService = aboutService;
        }

        public async Task<IActionResult> Hakkımızda()
        {
            var values = await _aboutService.TGetListAsync();
            var valueList = _mapper.Map<List<ResultAboutDto>>(values);
            return View(valueList);
        }
    }
}
