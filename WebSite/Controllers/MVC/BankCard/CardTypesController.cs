using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;
using MediaLib.DTO;
using MediaLib.Helpers;
using MediaLib.Services;
using System.Security.Claims;

[Authorize]
[Route("CardTypes")]
public class CardTypesController : Controller
{
    private readonly CardTypesService _cardTypesService;
    private readonly PaymentSystemService _paymentSystemService;
    private readonly DocumentService _documentService;

    public CardTypesController(
        CardTypesService cardTypesService,
        PaymentSystemService paymentSystemService,
        DocumentService documentService)
    {
        _cardTypesService = cardTypesService;
        _paymentSystemService = paymentSystemService;
        _documentService = documentService;
    }

    private Guid GetOwnerId()
    {
        return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var cardTypes = await _cardTypesService.GetAllWithAvatarsAsync();
        return View(cardTypes);
    }

    [HttpGet("Add")]
    public async Task<IActionResult> Add()
    {
        ViewBag.PaymentSystemTypes = new SelectList(await _paymentSystemService.GetAllAsync(), "Id", "Name");
        return View();
    }

    [HttpPost("Add")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(
        CardTypeDetailDTO cardTypeDto,
        IFormFile? avatarFile,
        List<IFormFile>? mediaFiles,
        List<IFormFile>? documentFiles)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.PaymentSystemTypes = new SelectList(await _paymentSystemService.GetAllAsync(), "Id", "Name");
            return View(cardTypeDto);
        }

        var ownerId = GetOwnerId();

        var avatar = await FileHelper.CreateDTOFromUploadedFileAsync<AvatarDTO>(avatarFile);
        var mediaDTOs = await FileHelper.CreateDTOListFromUploadedFilesAsync<MediaDataDTO>(mediaFiles);
        var documentDTOs = await FileHelper.CreateDTOListFromUploadedFilesAsync<DocumentsDTO>(documentFiles);

        await _cardTypesService.AddAsync(cardTypeDto, avatar, mediaDTOs, documentDTOs, ownerId);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var cardType = await _cardTypesService.GetByIdWithDetailsAsync(id);
        if (cardType == null)
            return NotFound();

        ViewBag.PaymentSystemTypes = new SelectList(await _paymentSystemService.GetAllAsync(), "Id", "Name");
        return View(cardType);
    }

    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        CardTypeDetailDTO cardTypeDto,
        IFormFile? avatarFile,
        List<IFormFile>? mediaFiles,
        List<IFormFile>? documentFiles,
        Guid? PrimaryMediaId,
        List<Guid>? MediaToDelete,
        Guid? PrimaryDocumentId,
        List<Guid>? DocumentsToDelete)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.PaymentSystemTypes = new SelectList(await _paymentSystemService.GetAllAsync(), "Id", "Name");
            return View(cardTypeDto);
        }

        var ownerId = GetOwnerId();

        var avatar = await FileHelper.CreateDTOFromUploadedFileAsync<AvatarDTO>(avatarFile);

        await _cardTypesService.UpdateAsync(
            cardTypeDto,
            avatar,
            mediaFiles,
            documentFiles,
            PrimaryMediaId,
            MediaToDelete,
            PrimaryDocumentId,
            DocumentsToDelete,
            ownerId);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _cardTypesService.DeleteAsync(id);
        TempData["Message"] = $"Card Type with ID {id} deleted successfully.";
        return RedirectToAction("Index");
    }

    [HttpGet("Details/{id}")]
    public async Task<IActionResult> Details(Guid id)
    {
        var cardType = await _cardTypesService.GetByIdWithDetailsAsync(id);
        if (cardType == null)
            return NotFound();

        return View(cardType);
    }

    [HttpGet("DownloadDocument/{id}")]
    public async Task<IActionResult> DownloadDocument(Guid id)
    {
        var document = await _documentService.GetDocumentByIdAsync(id);
        if (document == null)
        {
            return NotFound("Document not found.");
        }

        if (document.Content == null)
        {
            return BadRequest("Document content is empty.");
        }

        return File(document.Content, "application/octet-stream", document.Name + document.Extension);
    }
}
