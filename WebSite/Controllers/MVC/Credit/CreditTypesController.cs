using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;
using MediaLib.DTO;
using MediaLib.Helpers;
using MediaLib.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebSite.Controllers.MVC
{
    [Authorize(Policy = "CookiePolicy")]
    [AuthorizeRoles("Admin", "User")]
    [Controller]
    public class CreditTypesController : Controller
    {
        private readonly CreditTypeService _creditTypeService;
        private readonly DocumentService _documentService;

        public CreditTypesController(
            CreditTypeService creditTypeService,
            DocumentService documentService)
        {
            _creditTypeService = creditTypeService;
            _documentService = documentService;
        }

        private Guid GetOwnerId()
        {
            return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var creditTypes = await _creditTypeService.GetAllAsync();
            return View(creditTypes);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreditTypeDTO creditTypeDto, List<IFormFile>? documentFiles)
        {
            if (!ModelState.IsValid)
                return View(creditTypeDto);

            var documents = await FileHelper.CreateDTOListFromUploadedFilesAsync<DocumentsDTO>(documentFiles);
            var ownerId = GetOwnerId();

            await _creditTypeService.AddAsync(creditTypeDto, documents, ownerId);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var creditType = await _creditTypeService.GetByIdWithDetailsAsync(id);
            if (creditType == null)
                return NotFound();

            return View(creditType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            CreditTypeDTO creditTypeDto,
            List<IFormFile>? newDocumentFiles,
            List<Guid>? documentsToDelete,
            Guid? primaryDocumentId)
        {
            if (!ModelState.IsValid)
                return View(creditTypeDto);

            var ownerId = GetOwnerId();

            await _creditTypeService.UpdateAsync(
                creditTypeDto,
                newDocumentFiles,
                documentsToDelete,
                primaryDocumentId,
                ownerId);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _creditTypeService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var creditType = await _creditTypeService.GetByIdWithDetailsAsync(id);
            if (creditType == null)
                return NotFound();

            return View(creditType);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDocument(Guid documentId)
        {
            await _creditTypeService.DeleteDocumentAsync(documentId);
            TempData["Message"] = $"Document with ID {documentId} deleted successfully.";
            return RedirectToAction(nameof(Index));
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
}
