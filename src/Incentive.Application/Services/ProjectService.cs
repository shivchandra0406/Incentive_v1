using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Incentive.Application.Common.Models;
using Incentive.Application.DTOs;
using Incentive.Application.Interfaces;
using Incentive.Core.Entities;
using Incentive.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Incentive.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ITenantService _tenantService;

        public ProjectService(AppDbContext dbContext, IMapper mapper, ITenantService tenantService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _tenantService = tenantService;
        }

        public async Task<ProjectDto> GetProjectByIdAsync(Guid id)
        {
            var project = await _dbContext.Projects
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
        {
            var projects = await _dbContext.Projects
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProjectDto>>(projects);
        }

        public async Task<PaginatedList<ProjectDto>> GetPaginatedProjectsAsync(int pageNumber, int pageSize, string searchTerm = null)
        {
            var query = _dbContext.Projects.AsNoTracking();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => !p.IsDeleted &&
                    p.Name.Contains(searchTerm) ||
                    p.Description.Contains(searchTerm) ||
                    p.Location.Contains(searchTerm) ||
                    p.PropertyType.Contains(searchTerm) ||
                    p.Status.Contains(searchTerm) ||
                    p.AgentName.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(p => p.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var projectDtos = _mapper.Map<List<ProjectDto>>(items);

            return new PaginatedList<ProjectDto>(
                projectDtos,
                totalCount,
                pageNumber,
                pageSize);
        }

        public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto)
        {
            var tenantId = _tenantService.GetCurrentTenantId();

            var project = _mapper.Map<Project>(createProjectDto);
            project.TenantId = tenantId;

            _dbContext.Projects.Add(project);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto> UpdateProjectAsync(Guid id, UpdateProjectDto updateProjectDto)
        {
            var project = await _dbContext.Projects
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                return null;
            }

            _mapper.Map(updateProjectDto, project);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<bool> DeleteProjectAsync(Guid id)
        {
            var project = await _dbContext.Projects
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                return false;
            }

            _dbContext.Projects.Remove(project);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<ProjectMinimalDto>> GetProjectsMinimalAsync()
        {
            var projects = await _dbContext.Projects
                .Where(x => !x.IsDeleted)
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .Select(p => new ProjectMinimalDto
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToListAsync();

            return projects;
        }
    }
}
