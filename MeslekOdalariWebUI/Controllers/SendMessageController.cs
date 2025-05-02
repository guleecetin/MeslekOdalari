using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.MessageDtos;
using MeslekOdalari.Entity.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MeslekOdalariWebUI.Controllers
{
    public class SendMessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;

        public SendMessageController(IMapper mapper, IMessageService messageService)
        {
            _mapper = mapper;
            _messageService = messageService;
        }

        [HttpPost]
        public async Task< IActionResult> SendMessage(CreateMessageDto createMessageDto)
        {
            var newMessage = _mapper.Map<Message>(createMessageDto);
            await _messageService.TCreateAsync(newMessage);
            return RedirectToAction("Index","Default");
        }
    }
}
