using EquipHostWebApi.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipHostWebApi.Application.Interfaces
{
    public interface IFacilityRepository
    {
        public Task<bool> FacilityExistAsync(string code);
        public Task<Result<bool>> HasSufficientAreaAsync(string facilityCode, string equipmentCode,int quantity);
    }
}
