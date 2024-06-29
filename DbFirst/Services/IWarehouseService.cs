using DbFirst.DTO;

namespace DbFirst.Services
{
    public interface IWarehouseService
    {
        int AddProductWarehouse(WarehouseDTO warehouseDto);
        public int AddProductWarehouseProc(WarehouseDTO warehouseDto);
    }
}
