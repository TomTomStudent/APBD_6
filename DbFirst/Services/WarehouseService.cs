using DbFirst.Context;
using DbFirst.DTO;
using DbFirst.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DbFirst.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly APBD_6Context _context;

        public WarehouseService(APBD_6Context context)
        {
            _context = context;
        }

        public int AddProductWarehouse(WarehouseDTO warehouseDto)
        {
            var product = _context.Products.Find(warehouseDto.IdProduct);
            if (product == null)
            {
                throw new Exception("Product does not exist.");
            }

            var warehouse = _context.Warehouses.Find(warehouseDto.IdWarehouse);
            if (warehouse == null)
            {
                throw new Exception("Warehouse does not exist.");
            }

            if (warehouseDto.Amount <= 0)
            {
                throw new Exception("Amount must be greater than zero.");
            }

            var order = _context.Orders
                .FirstOrDefault(o => o.IdProduct == warehouseDto.IdProduct &&
                                     o.Amount == warehouseDto.Amount &&
                                     o.CreatedAt < warehouseDto.CreatedAt);
            if (order == null)
            {
                throw new Exception("No matching product purchase order found.");
            }

            var existingProductWarehouse = _context.ProductWarehouses
                .FirstOrDefault(pw => pw.IdOrder == order.IdOrder);
            if (existingProductWarehouse != null)
            {
                throw new Exception("The order has already been completed.");
            }

            order.FulfilledAt = DateTime.Now;
            _context.Orders.Update(order);

            var productWarehouse = new ProductWarehouse
            {
                IdProduct = warehouseDto.IdProduct,
                IdWarehouse = warehouseDto.IdWarehouse,
                IdOrder = order.IdOrder,
                Amount = warehouseDto.Amount,
                Price = product.Price * warehouseDto.Amount,
                CreatedAt = DateTime.Now
            };
            _context.ProductWarehouses.Add(productWarehouse);
            _context.SaveChanges();

            return productWarehouse.IdProductWarehouse;
        }

        public int AddProductWarehouseProc(WarehouseDTO warehouseDto)
        {
            var idParam = new SqlParameter("@IdProduct", warehouseDto.IdProduct);
            var warehouseParam = new SqlParameter("@IdWarehouse", warehouseDto.IdWarehouse);
            var amountParam = new SqlParameter("@Amount", warehouseDto.Amount);
            var createdAtParam = new SqlParameter("@CreatedAt", warehouseDto.CreatedAt);

            var newIdParam = new SqlParameter
            {
                ParameterName = "@NewId",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            _context.Database.ExecuteSqlRaw(
                "EXEC AddProductToWarehouse @IdProduct, @IdWarehouse, @Amount, @CreatedAt, @NewId OUT",
                idParam, warehouseParam, amountParam, createdAtParam, newIdParam);

            return (int)newIdParam.Value;
        }
    }
}
