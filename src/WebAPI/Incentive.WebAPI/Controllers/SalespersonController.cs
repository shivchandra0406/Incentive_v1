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
    public class SalespersonController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public SalespersonController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<PaginatedList<Salesperson>>>> GetSalespeople([FromQuery] PaginationRequest request)
        {
            var query = _unitOfWork.Repository<Salesperson>().AsQueryable()
                .Where(s => !s.IsDeleted);

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(s => 
                    s.FirstName.Contains(request.SearchTerm) || 
                    s.LastName.Contains(request.SearchTerm) || 
                    s.Email.Contains(request.SearchTerm));
            }

            var paginatedList = await PaginatedList<Salesperson>.CreateAsync(
                query, request.PageNumber, request.PageSize);

            return Ok(BaseResponse<PaginatedList<Salesperson>>.Success(paginatedList));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<Salesperson>>> GetSalesperson(Guid id)
        {
            var salesperson = await _unitOfWork.Repository<Salesperson>().GetByIdAsync(id);
            if (salesperson == null || salesperson.IsDeleted)
            {
                return NotFound(BaseResponse<Salesperson>.Failure($"Salesperson with ID {id} not found"));
            }

            return Ok(BaseResponse<Salesperson>.Success(salesperson));
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<Salesperson>>> CreateSalesperson(CreateSalespersonRequest request)
        {
            var salesperson = new Salesperson
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                EmployeeId = request.EmployeeId,
                HireDate = request.HireDate,
                IsActive = true
            };

            await _unitOfWork.Repository<Salesperson>().AddAsync(salesperson);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSalesperson), new { id = salesperson.Id }, BaseResponse<Salesperson>.Success(salesperson));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponse<Salesperson>>> UpdateSalesperson(Guid id, UpdateSalespersonRequest request)
        {
            var salesperson = await _unitOfWork.Repository<Salesperson>().GetByIdAsync(id);
            if (salesperson == null || salesperson.IsDeleted)
            {
                return NotFound(BaseResponse<Salesperson>.Failure($"Salesperson with ID {id} not found"));
            }

            salesperson.FirstName = request.FirstName;
            salesperson.LastName = request.LastName;
            salesperson.Email = request.Email;
            salesperson.PhoneNumber = request.PhoneNumber;
            salesperson.EmployeeId = request.EmployeeId;
            salesperson.HireDate = request.HireDate;
            salesperson.IsActive = request.IsActive;

            await _unitOfWork.Repository<Salesperson>().UpdateAsync(salesperson);
            await _unitOfWork.SaveChangesAsync();

            return Ok(BaseResponse<Salesperson>.Success(salesperson));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponse<object>>> DeleteSalesperson(Guid id)
        {
            var salesperson = await _unitOfWork.Repository<Salesperson>().GetByIdAsync(id);
            if (salesperson == null || salesperson.IsDeleted)
            {
                return NotFound(BaseResponse<object>.Failure($"Salesperson with ID {id} not found"));
            }

            await _unitOfWork.Repository<Salesperson>().DeleteAsync(salesperson);
            await _unitOfWork.SaveChangesAsync();

            return Ok(BaseResponse<object>.Success(null, "Salesperson deleted successfully"));
        }
    }
}
