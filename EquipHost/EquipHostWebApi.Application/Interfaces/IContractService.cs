using EquipHostWebApi.Application.DTOs;
using EquipHostWebApi.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipHostWebApi.Application.Interfaces
{
    public interface IContractService
    {
        Task<Result<CreateContractDto>> CreateContractAsync(CreateContractDto contractDto);
        Task<PageResultResponse<GetContractDto>> GetContractsAsync(ContractFilter filter);
    }
}
