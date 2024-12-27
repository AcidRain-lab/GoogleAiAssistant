using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;

namespace WebSite.Controllers.MVC.DepositTerm
{
    [Authorize]
    [Route("DepositTerms")]
    public class DepositTermsController : Controller
    {
        private readonly DepositTermService _depositTermService;

        public DepositTermsController(DepositTermService depositTermService)
        {
            _depositTermService = depositTermService;
        }

        [HttpGet("Index/{depositTypeId}")]
        public async Task<IActionResult> Index(Guid depositTypeId)
        {
            var terms = await _depositTermService.GetByDepositTypeIdAsync(depositTypeId);
            ViewBag.DepositTypeId = depositTypeId;
            return View(terms);
        }

        [HttpGet("Add/{depositTypeId}")]
        public IActionResult Add(Guid depositTypeId)
        {
            var term = new DepositTermDTO { DepositTypeId = depositTypeId };
            return View(term);
        }

        [HttpPost("Add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(DepositTermDTO termDto)
        {
            if (!ModelState.IsValid) return View(termDto);
            await _depositTermService.AddAsync(termDto);
            return RedirectToAction(nameof(Index), new { depositTypeId = termDto.DepositTypeId });
        }

        [HttpGet("Edit/{id}/{depositTypeId}")]
        public async Task<IActionResult> Edit(Guid id, Guid depositTypeId)
        {
            var term = await _depositTermService.GetByIdAsync(id);
            if (term == null) return NotFound();
            ViewBag.DepositTypeId = depositTypeId;
            return View(term);
        }

        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepositTermDTO termDto)
        {
            if (!ModelState.IsValid) return View(termDto);
            await _depositTermService.UpdateAsync(termDto);
            return RedirectToAction(nameof(Index), new { depositTypeId = termDto.DepositTypeId });
        }

        [HttpPost("Delete/{id}/{depositTypeId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, Guid depositTypeId)
        {
            await _depositTermService.DeleteAsync(id);
            return RedirectToAction(nameof(Index), new { depositTypeId });
        }
    }
}
