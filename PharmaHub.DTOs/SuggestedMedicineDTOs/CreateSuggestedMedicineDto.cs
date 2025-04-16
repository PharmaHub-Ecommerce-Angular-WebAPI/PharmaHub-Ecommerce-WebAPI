using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaHub.DTOs.SuggestedMedicineDTOs
{
    public class CreateSuggestedMedicineDto
    {
        public string Name { get; set; } = string.Empty;
        public short Strength { get; set; } = default;
    }
}
