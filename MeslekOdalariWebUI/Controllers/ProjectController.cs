using AutoMapper;
using MeslekOdalari.Business.Abstract;
using MeslekOdalari.Dto.Dtos.ProjectDtos;
using MeslekOdalari.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace MeslekOdalariWebUI.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;


        public ProjectController(IProjectService projectService, IMapper mapper)
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
        public async Task<IActionResult> DeleteProject(ObjectId id)
        {
            await _projectService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }

        public IActionResult CreateProject()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateProject(CreateProjectDto createProjectDto)
        {
            var newValue = _mapper.Map<Project>(createProjectDto);
            await _projectService.TCreateAsync(newValue);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> UpdateProject(ObjectId id)
        {
            var value = await _projectService.TGetByIdAsync(id);
            var updateValue = _mapper.Map<UpdateProjectDto>(value);
            return View(updateValue);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProject(UpdateProjectDto updateProjectDto)
        {
            var updateValue = _mapper.Map<Project>(updateProjectDto);
            await _projectService.TUpdateAsync(updateValue);
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> ProjectDetails(ObjectId id)
        {
            var value = await _projectService.TGetByIdAsync(id);
            var project = _mapper.Map<ResultProjectDto>(value);
            return View(project);
        }
    }
}
