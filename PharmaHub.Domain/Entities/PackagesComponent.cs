using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaHub.Domain.Entities
{
    public class PackagesComponent
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public Guid PackageId { get; set; } = Guid.NewGuid();

       // public string PackageName { get; set; }


    }
}
