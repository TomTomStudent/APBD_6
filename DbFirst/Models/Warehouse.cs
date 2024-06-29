using System;
using System.Collections.Generic;

namespace DbFirst.Models
{
    public partial class Warehouse
    {
        public Warehouse()
        {
            ProductWarehouses = new HashSet<ProductWarehouse>();
        }

        public int IdWarehouse { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;

        public virtual ICollection<ProductWarehouse> ProductWarehouses { get; set; }
    }
}
