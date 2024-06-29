using System;
using System.Collections.Generic;

namespace DbFirst.Models
{
    public partial class Product
    {
        public Product()
        {
            Orders = new HashSet<Order>();
            ProductWarehouses = new HashSet<ProductWarehouse>();
        }

        public int IdProduct { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ProductWarehouse> ProductWarehouses { get; set; }
    }
}
