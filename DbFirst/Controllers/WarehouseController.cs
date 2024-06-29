using DbFirst.DTO;
using DbFirst.Services;
using Microsoft.AspNetCore.Mvc;

namespace DbFirst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        public WarehouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpPost("add")]
        public IActionResult AddProductWarehouse([FromBody] WarehouseDTO warehouseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                int newId = _warehouseService.AddProductWarehouse(warehouseDto);
                return Ok(new { IdProductWarehouse = newId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("proc-add")]
        public IActionResult AddProductWarehouseProc([FromBody] WarehouseDTO warehouseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                int newId = _warehouseService.AddProductWarehouseProc(warehouseDto);
                return Ok(new { IdProductWarehouse = newId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
