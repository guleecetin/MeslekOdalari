using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.RoomsDto;
using Microsoft.AspNetCore.Mvc;

namespace MeslekOdalariWebUI.ViewComponents.Default_Index
{
    public class _DefaultRooms:ViewComponent
    {
        private readonly IRoomsService _roomsService;
        private readonly IMapper _mapper;

        public _DefaultRooms(IMapper mapper, IRoomsService roomsService)
        {
            _mapper = mapper;
            _roomsService = roomsService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var values = await _roomsService.TGetListAsync();
            var rooms = _mapper.Map<List<ResultRoomsDto>>(values);
            return View(rooms);
        }
    }
}
