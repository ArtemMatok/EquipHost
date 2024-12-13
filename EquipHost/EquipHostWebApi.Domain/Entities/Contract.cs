using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipHostWebApi.Domain.Entities
{
    public class Contract
    {
        public int ContractId { get; set; }
        public string FacilityCode { get; set; } 
        [ForeignKey("FacilityCode")]
        public Facility Facility { get; set; }
        public string EquipmentCode { get; set; } 
        [ForeignKey("EquipmentCode")]
        public EquipmentType EquipmentType { get; set; }
        public int Quantity { get; set; } 
    }
}
