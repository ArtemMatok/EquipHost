using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipHostWebApi.Application.DTOs
{
    public class CreateContractDto
    {
        public string FacilityCode { get; set; }
        public string EquipmentCode { get; set; }
        public int Quantity { get; set; }
    }
}
