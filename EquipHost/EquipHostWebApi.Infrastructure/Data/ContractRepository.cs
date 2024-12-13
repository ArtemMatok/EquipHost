using EquipHostWebApi.Application;
using EquipHostWebApi.Application.Interfaces;
using EquipHostWebApi.Application.Responses;
using EquipHostWebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipHostWebApi.Infrastructure.Data
{
    public class ContractRepository(
        ApplicationDbContext _context
    ): IContractRepository
    {
        public async Task<Result<Contract>> CreateContractAsync(Contract contract)
        {
            try
            {
                await _context.Contracts.AddAsync(contract);
                await _context.SaveChangesAsync();

                return Result<Contract>.Success(contract);
            }
            catch (Exception ex)
            {
                return Result<Contract>.Failure(ex.Message);
            }
        }

        public async Task<PageResultResponse<Contract>> GetContractsAsync(ContractFilter filter)
        {
            var contracts = _context.Contracts
                .Include(x => x.Facility)
                .Include(x => x.EquipmentType)
                .AsQueryable();

            int totalCount = await contracts.CountAsync();

            contracts = contracts
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize);

         var contractList = await contracts.ToListAsync();

            return new PageResultResponse<Contract>(contractList, totalCount, filter.PageNumber, filter.PageSize);
        }
    }
}
