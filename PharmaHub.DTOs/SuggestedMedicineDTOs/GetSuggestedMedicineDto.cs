using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaHub.DTOs.SuggestedMedicineDTOs
{
    public class GetSuggestedMedicineDto
    {
        public Guid Id { get; set; } 

        public string Name { get; set; } 

        public short? Strength { get; set; }
    }
}
