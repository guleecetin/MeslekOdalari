using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.NewsDtos;
using MeslekOdalari.Dto.Dtos.RoomsDto;
using MeslekOdalari.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace MeslekOdalariWebUI.Controllers
{
    public class RoomsController : Controller
    {
        private readonly IRoomsService _roomsService;
        private readonly IMapper _mapper;

        public RoomsController(IMapper mapper, IRoomsService roomsService)
        {
            _mapper = mapper;
            _roomsService = roomsService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _roomsService.TGetListAsync();
            var valueList = _mapper.Map<List<ResultRoomsDto>>(values);
            return View(valueList);
        }
        public async Task<IActionResult> DeleteRooms(ObjectId id)
        {
            await _roomsService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }

        public IActionResult CreateRooms()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRooms(CreateRoomsDto createRoomsDto)
        {
            var newValue = _mapper.Map<Rooms>(createRoomsDto);
            await _roomsService.TCreateAsync(newValue);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> UpdateRooms(ObjectId id)
        {
            var value = await _roomsService.TGetByIdAsync(id);
            var updateValue = _mapper.Map<UpdateRoomsDto>(value);
            return View(updateValue);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateRooms(UpdateRoomsDto updateRoomsDto)
        {
            var updateValue = _mapper.Map<Rooms>(updateRoomsDto);
            await _roomsService.TUpdateAsync(updateValue);
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> RoomsDetails(ObjectId id)
        {
            var value = await _roomsService.TGetByIdAsync(id);
            var rooms = _mapper.Map<ResultRoomsDto>(value);
            return View(rooms);
        }
    }
}
