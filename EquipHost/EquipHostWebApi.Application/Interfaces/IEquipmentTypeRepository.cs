using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipHostWebApi.Application.Interfaces
{
    public interface IEquipmentTypeRepository 
    {
        Task<bool> EquipmentTypeExistAsync(string code);
    }
}
