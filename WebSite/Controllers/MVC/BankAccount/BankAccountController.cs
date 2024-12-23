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
            var accounts = await _bankAccountService.GetByClientIdAsync(clientId);
            ViewBag.ClientId = clientId;
            return View(accounts);
        }

        public IActionResult Add(Guid clientId)
        {
            var account = new BankAccountDTO
            {
                ClientId = clientId
            };
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(BankAccountDTO accountDto)
        {
            if (!ModelState.IsValid)
                return View(accountDto);

            await _bankAccountService.CreateAsync(accountDto);
            return RedirectToAction(nameof(Index), new { clientId = accountDto.ClientId });
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var account = await _bankAccountService.GetByIdAsync(id);
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BankAccountDTO accountDto)
        {
            if (!ModelState.IsValid)
                return View(accountDto);

            await _bankAccountService.UpdateAsync(accountDto);
            return RedirectToAction(nameof(Index), new { clientId = accountDto.ClientId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id, Guid clientId)
        {
            await _bankAccountService.DeleteAsync(id);
            return RedirectToAction(nameof(Index), new { clientId });
        }
    }
}
