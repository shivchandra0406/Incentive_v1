using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Incentive.Application.Common.Models;
using Incentive.Application.DTOs;

namespace Incentive.Application.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectDto> GetProjectByIdAsync(Guid id);
        Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
        Task<PaginatedList<ProjectDto>> GetPaginatedProjectsAsync(int pageNumber, int pageSize, string searchTerm = null);
        Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto);
        Task<ProjectDto> UpdateProjectAsync(Guid id, UpdateProjectDto updateProjectDto);
        Task<bool> DeleteProjectAsync(Guid id);

        /// <summary>
        /// Gets a list of projects with minimal data (ID and Name only)
        /// </summary>
        /// <returns>List of projects with minimal data</returns>
        Task<IEnumerable<ProjectMinimalDto>> GetProjectsMinimalAsync();
    }
}
