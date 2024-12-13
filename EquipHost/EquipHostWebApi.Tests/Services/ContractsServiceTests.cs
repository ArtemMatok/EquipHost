using AutoMapper;
using Castle.Core.Logging;
using EquipHostWebApi.Application;
using EquipHostWebApi.Application.DTOs;
using EquipHostWebApi.Application.Interfaces;
using EquipHostWebApi.Application.Responses;
using EquipHostWebApi.Application.Services;
using EquipHostWebApi.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace EquipHostWebApi.Tests.Services
{
    public class ContractsServiceTests
    {
        private readonly Mock<IContractRepository> _contractRepositoryMock;
        private readonly Mock<IFacilityRepository> _facilityRepositoryMock;
        private readonly Mock<IEquipmentTypeRepository> _equipmentTypeRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<ContractService>> _loggerMock;
        private readonly Mock<IBackgroundTaskQueue> _backgroundTaskQueueMock;
        private readonly ContractService _contractService;

        public ContractsServiceTests()
        {
            _contractRepositoryMock = new Mock<IContractRepository>();
            _facilityRepositoryMock = new Mock<IFacilityRepository>();
            _equipmentTypeRepositoryMock = new Mock<IEquipmentTypeRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<ContractService>>();
            _backgroundTaskQueueMock = new Mock<IBackgroundTaskQueue>(); 

            _contractService = new ContractService(
                _contractRepositoryMock.Object,
                _facilityRepositoryMock.Object,
                _equipmentTypeRepositoryMock.Object,
                _mapperMock.Object,
                _backgroundTaskQueueMock.Object, 
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task CreateContractAsync_FacilityNotFound_ReturnsFailure()
        {
            // Arrange
            var contractDto = new CreateContractDto { FacilityCode = "InvalidFacilityCode", EquipmentCode = "EQ001", Quantity = 5 };
            _facilityRepositoryMock
                .Setup(repo => repo.FacilityExistAsync(contractDto.FacilityCode))
                .ReturnsAsync(false);

            // Act
            var result = await _contractService.CreateContractAsync(contractDto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Facility wasn`t found");
        }

        [Fact]
        public async Task CreateContractAsync_EquipmentTypeNotFound_ReturnsFailure()
        {
            // Arrange
            var contractDto = new CreateContractDto { FacilityCode = "FAC001", EquipmentCode = "InvalidCode", Quantity = 5 };
            _facilityRepositoryMock
                .Setup(repo => repo.FacilityExistAsync(contractDto.FacilityCode))
                .ReturnsAsync(true);
            _equipmentTypeRepositoryMock
                .Setup(repo => repo.EquipmentTypeExistAsync(contractDto.EquipmentCode))
                .ReturnsAsync(false);

            // Act
            var result = await _contractService.CreateContractAsync(contractDto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Equipment type wasn`t found");
        }

        [Fact]
        public async Task CreateContractAsync_InsufficientArea_ReturnsFailure()
        {
            // Arrange
            var contractDto = new CreateContractDto { FacilityCode = "FAC001", EquipmentCode = "EQ001", Quantity = 5 };
            _facilityRepositoryMock
                .Setup(repo => repo.FacilityExistAsync(contractDto.FacilityCode))
                .ReturnsAsync(true);
            _equipmentTypeRepositoryMock
                .Setup(repo => repo.EquipmentTypeExistAsync(contractDto.EquipmentCode))
                .ReturnsAsync(true);
            _facilityRepositoryMock
                .Setup(repo => repo.HasSufficientAreaAsync(contractDto.FacilityCode, contractDto.EquipmentCode, contractDto.Quantity))
                .ReturnsAsync(Result<bool>.Failure("Insufficient area"));

            // Act
            var result = await _contractService.CreateContractAsync(contractDto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Insufficient area");
        }

        [Fact]
        public async Task CreateContractAsync_ValidRequest_ReturnsSuccess()
        {
            // Arrange
            var contractDto = new CreateContractDto { FacilityCode = "FAC001", EquipmentCode = "EQ001", Quantity = 5 };
            var contract = new Contract();
            _facilityRepositoryMock
                .Setup(repo => repo.FacilityExistAsync(contractDto.FacilityCode))
                .ReturnsAsync(true);
            _equipmentTypeRepositoryMock
                .Setup(repo => repo.EquipmentTypeExistAsync(contractDto.EquipmentCode))
                .ReturnsAsync(true);
            _facilityRepositoryMock
                .Setup(repo => repo.HasSufficientAreaAsync(contractDto.FacilityCode, contractDto.EquipmentCode, contractDto.Quantity))
                .ReturnsAsync(Result<bool>.Success(true));
            _mapperMock
                .Setup(mapper => mapper.Map<Contract>(contractDto))
                .Returns(contract);
            _contractRepositoryMock
                .Setup(repo => repo.CreateContractAsync(contract))
                .ReturnsAsync(Result<Contract>.Success(contract));

            // Act
            var result = await _contractService.CreateContractAsync(contractDto);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(contractDto);
        }

        [Fact]
        public async Task GetContractsAsync_ValidRequest_ReturnsPaginatedResults()
        {
            // Arrange
            var filter = new ContractFilter { PageNumber = 1, PageSize = 10 };
            var items = new List<Contract>
            {
                new Contract()
            };
            var contracts = new PageResultResponse<Contract>(items, items.Count, 1, 10);

            var contractsDto = new List<GetContractDto> { new GetContractDto(), new GetContractDto() };

            _contractRepositoryMock
                .Setup(repo => repo.GetContractsAsync(filter))
                .ReturnsAsync(contracts);
            _mapperMock
                .Setup(mapper => mapper.Map<List<GetContractDto>>(contracts.Items))
                .Returns(contractsDto);

            // Act
            var result = await _contractService.GetContractsAsync(filter);

            // Assert
            result.Items.Should().BeEquivalentTo(contractsDto);
            result.TotalCount.Should().Be(1);
            result.PageSize.Should().Be(10);
        }
    }
}
