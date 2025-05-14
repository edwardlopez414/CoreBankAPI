using CoreBankAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost]
        [Route("create")]
        public IActionResult create(AccountDto model)
        {
            return Ok();
        }
        [HttpPost]
        [Route("balance")]
        public IActionResult balance()
        {
            return Ok();
        }
    }
}
