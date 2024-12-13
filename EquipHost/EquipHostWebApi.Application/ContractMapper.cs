using AutoMapper;
using EquipHostWebApi.Application.DTOs;
using EquipHostWebApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipHostWebApi.Application
{
    public class ContractMapper : Profile
    {
        public ContractMapper()
        {
            CreateMap<CreateContractDto, Contract>();
            CreateMap<Contract, GetContractDto>()
                .ForMember(dest => dest.FacilityName, opt => opt.MapFrom(src => src.Facility.Name))
                .ForMember(dest => dest.EquipmentTypeName, opt => opt.MapFrom(src => src.EquipmentType.Name));
        }
    }
}
