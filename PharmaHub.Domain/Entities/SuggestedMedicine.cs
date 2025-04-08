using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaHub.Domain.Entities
{
    public class SuggestedMedicine
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(150, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Range(0, short.MaxValue, ErrorMessage = "Strength must be zero or more.")]
        public short Strength { get; set; } = default;

    }
}
