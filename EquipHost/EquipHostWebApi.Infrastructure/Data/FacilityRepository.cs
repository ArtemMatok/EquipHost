using EquipHostWebApi.Application.Interfaces;
using EquipHostWebApi.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipHostWebApi.Infrastructure.Data
{
    public class FacilityRepository(
        ApplicationDbContext _context    
    ) : IFacilityRepository
    {
        public async Task<bool> FacilityExistAsync(string code)
        {
            var facility = await _context.Facilities.FindAsync(code);
            
            if(facility is null)return false;
            return true;
        }

        public async Task<Result<bool>> HasSufficientAreaAsync(string facilityCode, string equipmentCode ,int quantity)
        {
            var facility = await _context.Facilities.FindAsync(facilityCode);
            var equipment = await _context.EquipmentTypes.FindAsync(equipmentCode); 

            if (facility is null) return Result<bool>.Failure("Facility wasn`t found");
            if (equipment is null) return Result<bool>.Failure("Equipment type wasn`t found");

            var requiredArea = quantity * equipment.Area;

            var result = facility.StandardArea >= requiredArea;

            if (!result) return Result<bool>.Failure("There is not enough space to accommodate the equipment.");

            return Result<bool>.Success(true);
        }

    }
}
