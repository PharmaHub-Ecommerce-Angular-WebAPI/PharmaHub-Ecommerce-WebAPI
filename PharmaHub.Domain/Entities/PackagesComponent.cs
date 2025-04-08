using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaHub.Domain.Entities
{
    public class PackagesComponent
    {
        //will have composite key from Product id (forgien key) and ProductName

        public Guid PackageId { get; set; }
        public Product Product { get; set; }


        public string ComponentName { get; set; }

        //public Guid PackageId { get; set; } = Guid.NewGuid();

    }
}
