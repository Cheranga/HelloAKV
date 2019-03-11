using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace HelloAKV.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerRepository _customerRepository;

        public CustomersController(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var customers = await _customerRepository.GetCustomersAsync().ConfigureAwait(false);
            if (customers.Any())
            {
                return Ok(customers);
            }

            return NotFound("There are no customers");
        }
    }
}