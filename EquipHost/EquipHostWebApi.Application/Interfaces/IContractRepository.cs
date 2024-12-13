using EquipHostWebApi.Application.Responses;
using EquipHostWebApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipHostWebApi.Application.Interfaces
{
    public interface IContractRepository
    {
        Task<Result<Contract>> CreateContractAsync(Contract contract);
        Task<PageResultResponse<Contract>> GetContractsAsync(ContractFilter filter);
    }
}
