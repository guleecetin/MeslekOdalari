using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.ProjectDtos;
using MeslekOdalari.Dto.Dtos.RoomsDto;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace MeslekOdalariWebUI.Controllers
{
    public class UIProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;
        public UIProjectController(IProjectService projectService, IMapper mapper)
        {
            _projectService = projectService;
            _mapper = mapper;
        }


        public async Task<IActionResult> Index()
        {
            var values = await _projectService.TGetListAsync();
            var valueList = _mapper.Map<List<ResultProjectDto>>(values);
            return View(valueList);
        }

        // Kullanıcı tarafı - Proje Detayı
        public async Task<IActionResult> ProjectDetail(ObjectId id)
        {
            var value = await _projectService.TGetByIdAsync(id);
            var project = _mapper.Map<ResultProjectDto>(value);
            return View(project);
        }
    }
}
