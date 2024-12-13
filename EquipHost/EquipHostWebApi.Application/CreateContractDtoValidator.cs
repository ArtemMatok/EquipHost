using EquipHostWebApi.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipHostWebApi.Application
{
    public class CreateContractDtoValidator : AbstractValidator<CreateContractDto>
    {
        public CreateContractDtoValidator()
        {
            RuleFor(x => x.FacilityCode)
                .NotEmpty().WithMessage("Facility code is required");
            RuleFor(x => x.EquipmentCode)
                .NotEmpty().WithMessage("Equipment type code is required");
            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required")
                .GreaterThan(0).WithMessage("Quantity should be greater than 0");
        }
    }
}
