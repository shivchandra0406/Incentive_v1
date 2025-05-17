using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Incentive.Application.Common.Models;
using Incentive.Application.DTOs;
using Incentive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Incentive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// Get all projects
        /// </summary>
        /// <returns>List of projects</returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<ProjectDto>>), 200)]
        public async Task<ActionResult<BaseResponse<IEnumerable<ProjectDto>>>> GetAllProjects()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return Ok(BaseResponse<IEnumerable<ProjectDto>>.Success(projects));
        }

        /// <summary>
        /// Get paginated projects with optional search
        /// </summary>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10)</param>
        /// <param name="searchTerm">Optional search term</param>
        /// <returns>Paginated list of projects</returns>
        [HttpGet("paginated")]
        [ProducesResponseType(typeof(BaseResponse<PaginatedList<ProjectDto>>), 200)]
        public async Task<ActionResult<BaseResponse<PaginatedList<ProjectDto>>>> GetPaginatedProjects(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string searchTerm = null)
        {
            var paginatedProjects = await _projectService.GetPaginatedProjectsAsync(pageNumber, pageSize, searchTerm);
            return Ok(BaseResponse<PaginatedList<ProjectDto>>.Success(paginatedProjects));
        }

        /// <summary>
        /// Get project by ID
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <returns>Project details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<ProjectDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<string>), 404)]
        public async Task<ActionResult<BaseResponse<ProjectDto>>> GetProject(Guid id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound(BaseResponse<string>.Failure($"Project with ID {id} not found"));
            }

            return Ok(BaseResponse<ProjectDto>.Success(project));
        }

        /// <summary>
        /// Create a new project
        /// </summary>
        /// <param name="createProjectDto">Project creation data</param>
        /// <returns>Created project</returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<ProjectDto>), 201)]
        [ProducesResponseType(typeof(BaseResponse<string>), 400)]
        public async Task<ActionResult<BaseResponse<ProjectDto>>> CreateProject([FromBody] CreateProjectDto createProjectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(BaseResponse<string>.Failure("Invalid project data"));
            }

            var createdProject = await _projectService.CreateProjectAsync(createProjectDto);
            return CreatedAtAction(nameof(GetProject), new { id = createdProject.Id }, BaseResponse<ProjectDto>.Success(createdProject));
        }

        /// <summary>
        /// Update an existing project
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <param name="updateProjectDto">Project update data</param>
        /// <returns>Updated project</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseResponse<ProjectDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<string>), 400)]
        [ProducesResponseType(typeof(BaseResponse<string>), 404)]
        public async Task<ActionResult<BaseResponse<ProjectDto>>> UpdateProject(Guid id, [FromBody] UpdateProjectDto updateProjectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(BaseResponse<string>.Failure("Invalid project data"));
            }

            var updatedProject = await _projectService.UpdateProjectAsync(id, updateProjectDto);
            if (updatedProject == null)
            {
                return NotFound(BaseResponse<string>.Failure($"Project with ID {id} not found"));
            }

            return Ok(BaseResponse<ProjectDto>.Success(updatedProject));
        }

        /// <summary>
        /// Delete a project
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <returns>Success message</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseResponse<string>), 200)]
        [ProducesResponseType(typeof(BaseResponse<string>), 404)]
        public async Task<ActionResult<BaseResponse<string>>> DeleteProject(Guid id)
        {
            var result = await _projectService.DeleteProjectAsync(id);
            if (!result)
            {
                return NotFound(BaseResponse<string>.Failure($"Project with ID {id} not found"));
            }

            return Ok(BaseResponse<string>.Success("Project deleted successfully"));
        }

        /// <summary>
        /// Get minimal project data (ID and Name only)
        /// </summary>
        /// <returns>List of projects with minimal data</returns>
        [HttpGet("minimal")]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<ProjectMinimalDto>>), 200)]
        public async Task<ActionResult<BaseResponse<IEnumerable<ProjectMinimalDto>>>> GetProjectsMinimal()
        {
            var projects = await _projectService.GetProjectsMinimalAsync();
            return Ok(BaseResponse<IEnumerable<ProjectMinimalDto>>.Success(projects));
        }
    }
}
