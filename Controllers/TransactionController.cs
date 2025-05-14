using CoreBankAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        [HttpPost]
        [Route("deposits")]
        public IActionResult create(AccountDto model)
        {
            return Ok();
        }
        [HttpPost]
        [Route("withdrawals")]
        public IActionResult withdrawals(AccountDto model)
        {
            return Ok();
        }
    }
}
