using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.DateDtos;
using MeslekOdalari.Entity.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MeslekOdalariWebUI.Controllers
{
    public class UIDateController : Controller
    {
        private readonly IDateService _dateService;
        private readonly IMapper _mapper;

        public UIDateController(IMapper mapper, IDateService dateService)
        {
            _mapper = mapper;
            _dateService = dateService;
        }

        // Randevu alma formunu göster
        [HttpGet]
        public async Task< IActionResult> CreateDate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateDate(CreateDateDto createDateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createDateDto); // Kullanıcı hatalı veri girdiyse aynı sayfaya dön
            }

            var newDate = _mapper.Map<Date>(createDateDto); // DTO -> Entity
            await _dateService.TCreateAsync(newDate); // Veritabanına kaydet

            return RedirectToAction("Index", "Default"); 
        }



    }
}
