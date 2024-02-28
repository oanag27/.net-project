using Learn.Net.Modal;
using Learn.Net.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Learn.Net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService) {
            _customerService = customerService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> Get()
        {
            var data = await _customerService.GetAll();
            //no data
            if (data == null)
            {
                return NotFound();  // 404 Not Found -> response status code
            }
            return Ok(data);   // 200 OK -> response status code
        }
        [HttpGet("GetByCode")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var data = await _customerService.GetByCode(code);
            //no data
            if (data == null)
            {
                return NotFound();  // 404 Not Found -> response status code
            }
            return Ok(data);   // 200 OK -> response status code
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add(CustomerModal customerModal)
        {
            var data = await _customerService.Add(customerModal);
            //directly return response
            return Ok(data);   // 200 OK -> response status code
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(CustomerModal customerModal, string code)
        {
            var data = await _customerService.Update(customerModal, code);
            //directly return response
            return Ok(data);   // 200 OK -> response status code
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string code)
        {
            var data = await _customerService.Delete(code);
            //directly return response
            return Ok(data);   // 200 OK -> response status code
        }
    }
}
