using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipHostWebApi.Application.DTOs
{
    public class GetContractDto
    {
        public string FacilityName { get; set; }
        public string EquipmentTypeName { get; set; }
        public int Quantity { get; set; }
    }
}
