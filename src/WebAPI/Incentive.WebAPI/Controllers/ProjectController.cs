using System;
using System.Threading.Tasks;
using Incentive.Application.Common.Models;
using Incentive.Domain.Entities;
using Incentive.Ports.Repositories;
using Incentive.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Incentive.WebAPI.Controllers
{
    [Authorize]
    public class ProjectController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<PaginatedList<Project>>>> GetProjects([FromQuery] PaginationRequest request)
        {
            var query = _unitOfWork.Repository<Project>().AsQueryable()
                .Where(p => !p.IsDeleted);

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(p => 
                    p.Name.Contains(request.SearchTerm) || 
                    p.Description.Contains(request.SearchTerm) || 
                    p.Location.Contains(request.SearchTerm));
            }

            var paginatedList = await PaginatedList<Project>.CreateAsync(
                query, request.PageNumber, request.PageSize);

            return Ok(BaseResponse<PaginatedList<Project>>.Success(paginatedList));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<Project>>> GetProject(Guid id)
        {
            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(id);
            if (project == null || project.IsDeleted)
            {
                return NotFound(BaseResponse<Project>.Failure($"Project with ID {id} not found"));
            }

            return Ok(BaseResponse<Project>.Success(project));
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<Project>>> CreateProject(CreateProjectRequest request)
        {
            var project = new Project
            {
                Name = request.Name,
                Description = request.Description,
                Location = request.Location,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                TotalValue = request.TotalValue,
                IsActive = true
            };

            await _unitOfWork.Repository<Project>().AddAsync(project);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, BaseResponse<Project>.Success(project));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponse<Project>>> UpdateProject(Guid id, UpdateProjectRequest request)
        {
            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(id);
            if (project == null || project.IsDeleted)
            {
                return NotFound(BaseResponse<Project>.Failure($"Project with ID {id} not found"));
            }

            project.Name = request.Name;
            project.Description = request.Description;
            project.Location = request.Location;
            project.StartDate = request.StartDate;
            project.EndDate = request.EndDate;
            project.TotalValue = request.TotalValue;
            project.IsActive = request.IsActive;

            await _unitOfWork.Repository<Project>().UpdateAsync(project);
            await _unitOfWork.SaveChangesAsync();

            return Ok(BaseResponse<Project>.Success(project));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponse<object>>> DeleteProject(Guid id)
        {
            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(id);
            if (project == null || project.IsDeleted)
            {
                return NotFound(BaseResponse<object>.Failure($"Project with ID {id} not found"));
            }

            await _unitOfWork.Repository<Project>().DeleteAsync(project);
            await _unitOfWork.SaveChangesAsync();

            return Ok(BaseResponse<object>.Success(null, "Project deleted successfully"));
        }
    }
}
