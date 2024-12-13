using AutoMapper;
using EquipHostWebApi.Application.DTOs;
using EquipHostWebApi.Application.Interfaces;
using EquipHostWebApi.Application.Responses;
using EquipHostWebApi.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipHostWebApi.Application.Services
{
    public class ContractService(
        IContractRepository _contractRepository,
        IFacilityRepository _facilityRepository,
        IEquipmentTypeRepository _equipmentTypeRepository, 
        IMapper _mapper,
        IBackgroundTaskQueue _backgroundTaskQueue,
        ILogger<ContractService> _logger
    ) : IContractService
    {
        public async Task<Result<CreateContractDto>> CreateContractAsync(CreateContractDto contractDto)
        {
            if(!await _facilityRepository.FacilityExistAsync(contractDto.FacilityCode))
            {
                return Result<CreateContractDto>.Failure("Facility wasn`t found");
            }

            if (!await _equipmentTypeRepository.EquipmentTypeExistAsync(contractDto.EquipmentCode))
            {
                return Result<CreateContractDto>.Failure("Equipment type wasn`t found");
            }

            var sufficientArea = await _facilityRepository.HasSufficientAreaAsync(contractDto.FacilityCode, contractDto.EquipmentCode, contractDto.Quantity);
            if(!sufficientArea.IsSuccess)
            {
                return Result<CreateContractDto>.Failure(sufficientArea.ErrorMessage);
            }

            var contract = _mapper.Map<Contract>(contractDto);

            var result = await _contractRepository.CreateContractAsync(contract);    

            if(!result.IsSuccess)
            {
                return Result<CreateContractDto>.Failure(result.ErrorMessage);    
            }

            _backgroundTaskQueue.QueueBackgroundWorkItem(async token =>
            {
                 await LogContractCreationAsync(contract);
            });

            return Result<CreateContractDto>.Success(contractDto);
        }

        public async Task<PageResultResponse<GetContractDto>> GetContractsAsync(ContractFilter filter)
        {
            var contracts = await _contractRepository.GetContractsAsync(filter);

            var contractsDto = _mapper.Map<List<GetContractDto>>(contracts.Items);

            return new PageResultResponse<GetContractDto>(contractsDto, contracts.TotalCount, filter.PageNumber, filter.PageSize);
        }

        private async Task LogContractCreationAsync(Contract contract)
        {
            try
            {
                _logger.LogInformation($"Contract {contract.ContractId} for Facility {contract.FacilityCode} created successfully at {DateTime.UtcNow}.");

                // Imitation of a long asynchronous process
                await Task.Delay(1000);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while logging contract {contract.ContractId} creation.");
            }
        }
    }
}
