using EquipHostWebApi.Application;
using EquipHostWebApi.Application.DTOs;
using EquipHostWebApi.Application.Interfaces;
using EquipHostWebApi.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EquipHostWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractsController(
        IContractService _contractService
    ): ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateContract(CreateContractDto contractDto)
        {
            var result = await _contractService.CreateContractAsync(contractDto);

            return result.ToResponse();
        }

        [HttpGet]
        public async Task<IActionResult> GetContracts([FromQuery]ContractFilter filter)
        {
            return Ok(await _contractService.GetContractsAsync(filter));
        }
    }
}
