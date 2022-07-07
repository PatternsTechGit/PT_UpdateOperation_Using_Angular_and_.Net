using Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace BBBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : Controller
    {
        private readonly IAccountsService _accountsService;
        public AccountsController(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        [HttpGet]
        [Route("GetAllAccountsPaginated")]
        public async Task<ActionResult> GetAllAccountsPaginated([FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            try
            {
                var result = (await _accountsService.GetAllAccountsPaginated(pageIndex, pageSize));
                return new OkObjectResult(await _accountsService.GetAllAccountsPaginated(pageIndex, pageSize));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }

        [HttpPost]
        [Route("OpenAccount")]
        public async Task<ActionResult> OpenAccount(Account account)
        {
            try
            {
                await _accountsService.OpenAccount(account);
                return new OkObjectResult("New Account Created.");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }
    }
}
