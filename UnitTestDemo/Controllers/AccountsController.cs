using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UnitTestDemo.Service;

namespace UnitTestDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService accountsService;
        public AccountsController(IAccountsService accountsService)
        {
            this.accountsService = accountsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Guid? accountId)
        {
            if(accountId != null)
            {
                var result = await accountsService.Get(accountId.Value);
                return Ok(result);
            }
            else
            {
                var result = accountsService.GetAll();
                return Ok(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromQuery] string accountName, [FromQuery] string accountAddress, [FromQuery] long accountBalance)
        {
            var accountId=  await accountsService.Add(accountName, accountAddress, accountBalance);
            return Ok(accountId);
        }

        [HttpPatch]
        public async Task<IActionResult> Patch([FromQuery]Guid sourceAccountId, [FromQuery] Guid targetAcccountId, [FromQuery] long transferAmount)
        {
            await accountsService.Trasfer(sourceAccountId, targetAcccountId, transferAmount);
            return Ok();
        }
    }
}
