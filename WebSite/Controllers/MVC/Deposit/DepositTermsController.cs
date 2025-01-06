using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;

[Authorize]
[Route("DepositTerms")]
[Controller]
public class DepositTermsController : Controller
{
    private readonly DepositTermService _depositTermService;

    public DepositTermsController(DepositTermService depositTermService)
    {
        _depositTermService = depositTermService;
    }

    // GET: Add
    [HttpGet("Add/{depositTypeId}")]
    public IActionResult Add(Guid depositTypeId, string? returnUrl)
    {
        var term = new DepositTermDTO { DepositTypeId = depositTypeId };
        ViewBag.ReturnUrl = returnUrl;
        return View(term);
    }

    // POST: Add
    [HttpPost("Add/{depositTypeId}")]
    public async Task<IActionResult> Add(Guid depositTypeId, [FromForm] DepositTermDTO termDto, [FromQuery] string? returnUrl)
    {
        termDto.DepositTypeId = depositTypeId;

        if (!ModelState.IsValid)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(termDto);
        }

        await _depositTermService.AddAsync(termDto);
        return Redirect(returnUrl ?? Url.Action("Edit", "DepositTypes", new { id = termDto.DepositTypeId }));
    }

    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(Guid id, Guid depositTypeId, string? returnUrl)
    {
        var term = await _depositTermService.GetByIdAsync(id);
        if (term == null)
            return NotFound();

        term.DepositTypeId = depositTypeId;
        ViewBag.ReturnUrl = returnUrl;
        return View(term);
    }

    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, Guid depositTypeId, [FromForm] DepositTermDTO termDto, [FromQuery] string? returnUrl)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(termDto);
        }

        termDto.DepositTypeId = depositTypeId;
        await _depositTermService.UpdateAsync(termDto);

        return Redirect(returnUrl ?? Url.Action("Edit", "DepositTypes", new { id = depositTypeId }));
    }

    [HttpPost("Delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, Guid depositTypeId, string? returnUrl)
    {
        await _depositTermService.DeleteAsync(id);
        TempData["Message"] = "Deposit term deleted successfully.";
        return Redirect(returnUrl ?? Url.Action("Edit", "DepositTypes", new { id = depositTypeId }));
    }
}
