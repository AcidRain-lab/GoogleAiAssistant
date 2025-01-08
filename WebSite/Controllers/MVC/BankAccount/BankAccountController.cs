using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;

namespace WebSite.Controllers.MVC.BankAccount
{
    [Authorize]
    public class BankAccountController : Controller
    {
        private readonly BankAccountService _bankAccountService;

        public BankAccountController(BankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        public async Task<IActionResult> Index(Guid clientId)
        {
            if (clientId == Guid.Empty)
                return BadRequest("Invalid client ID.");

            var accounts = await _bankAccountService.GetByClientIdAsync(clientId);
            ViewBag.ClientId = clientId;
            return View(accounts);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid accountId)
        {
            var account = await _bankAccountService.GetAccountDetailsAsync(accountId);
            if (account == null)
                return NotFound("Account not found.");

            return View(account);
        }
    }
}
