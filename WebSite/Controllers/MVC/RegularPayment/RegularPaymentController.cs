using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;

namespace WebSite.Controllers.MVC
{
    [Authorize]
    public class RegularPaymentController : Controller
    {
        private readonly RegularPaymentService _paymentService;

        public RegularPaymentController(RegularPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<IActionResult> Index(Guid clientId)
        {
            var payments = await _paymentService.GetByClientIdAsync(clientId);
            ViewBag.ClientId = clientId;
            return View(payments);
        }

        public IActionResult Add(Guid clientId)
        {
            var payment = new RegularPaymentDTO { ClientId = clientId };
            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(RegularPaymentDTO paymentDto)
        {
            if (!ModelState.IsValid)
            {
                return View(paymentDto);
            }

            await _paymentService.CreateAsync(paymentDto);
            return RedirectToAction(nameof(Index), new { clientId = paymentDto.ClientId });
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var payment = await _paymentService.GetByIdAsync(id);
            if (payment == null)
                return NotFound();

            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RegularPaymentDTO paymentDto)
        {
            if (!ModelState.IsValid)
            {
                return View(paymentDto);
            }

            await _paymentService.UpdateAsync(paymentDto);
            return RedirectToAction(nameof(Index), new { clientId = paymentDto.ClientId });
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var payment = await _paymentService.GetByIdAsync(id);
            if (payment == null)
                return NotFound();

            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, Guid clientId)
        {
            await _paymentService.DeleteAsync(id);
            return RedirectToAction(nameof(Index), new { clientId });
        }
    }
}
