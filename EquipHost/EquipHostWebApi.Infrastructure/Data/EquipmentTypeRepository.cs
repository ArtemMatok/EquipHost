using EquipHostWebApi.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipHostWebApi.Infrastructure.Data
{
    public class EquipmentTypeRepository(
        ApplicationDbContext _context
    ) : IEquipmentTypeRepository
    {
        public async Task<bool> EquipmentTypeExistAsync(string code)
        {
            var type = await _context.EquipmentTypes.FindAsync(code);

            if (type is null) return false;
            return true;    
        }
    }
}
