using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.CounterDtos;
using Microsoft.AspNetCore.Mvc;

namespace MeslekOdalariWebUI.ViewComponents.Default_Index
{
    public class _DefaultCounter:ViewComponent
    {
        private readonly ICounterService _counterService;
        private readonly IMapper _mapper;

        public _DefaultCounter(IMapper mapper, ICounterService counterService)
        {
            _mapper = mapper;
            _counterService = counterService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _counterService.TGetListAsync();
            var counters = _mapper.Map<List<ResultCounterDto>>(values);
            return View(counters);
        }
    }
}
