using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipHostWebApi.Domain.Entities
{
    public class EquipmentType
    {
        [Key] 
        public string Code { get; set; }
        public string Name { get; set; }
        public double Area { get; set; } 
    }
}
