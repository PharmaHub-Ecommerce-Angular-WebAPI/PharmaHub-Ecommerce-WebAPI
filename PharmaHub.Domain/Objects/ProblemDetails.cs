using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaHub.Domain.Objects
{
    public class ProblemDetails
    {
        public string? name {  get; set; }
        public string? description { get; set; }

        public ProblemDetails() { }
        public ProblemDetails(string name)
        {
            this.name = name;
        }
        public ProblemDetails(string name, string description)
        {
            this.name = name;
            this.description = description;
        }
    }
}
