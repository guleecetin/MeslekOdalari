using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.QuestDtos;
using Microsoft.AspNetCore.Mvc;

namespace MeslekOdalariWebUI.ViewComponents.Default_Index
{
    public class _DefaultQuestion:ViewComponent
    {
        private readonly IQuestService _questService;
        private readonly IMapper _mapper;

        public _DefaultQuestion(IMapper mapper, IQuestService questService)
        {
            _mapper = mapper;
            _questService = questService;
        }

        public async Task <IViewComponentResult> InvokeAsync()
        {
            var values = await _questService.TGetListAsync();
            var questionList = _mapper.Map<List<ResultQuestDto>>(values);
            return View(questionList);
        }
    }
}
