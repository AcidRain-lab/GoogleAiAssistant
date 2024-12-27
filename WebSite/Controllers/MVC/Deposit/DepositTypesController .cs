using Microsoft.AspNetCore.Mvc;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;
using MediaLib.DTO;
using MediaLib.Helpers;
using Microsoft.AspNetCore.Authorization;
using MediaLib.Services;

namespace WebSite.Controllers.MVC
{
    [Authorize(Policy = "CookiePolicy")]
    [AuthorizeRoles("Admin", "User")]
    [Controller]
    public class DepositTypesController : Controller
    {
        private readonly DepositTypeService _depositTypeService;
        private readonly DocumentService _documentService;

        public DepositTypesController(
            DepositTypeService depositTypeService,
            DocumentService documentService)
        {
            _depositTypeService = depositTypeService;
            _documentService = documentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var depositTypes = await _depositTypeService.GetDepositTypesAsync();
            return View(depositTypes);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(DepositTypeDTO depositTypeDto, List<IFormFile>? documentFiles)
        {
            if (!ModelState.IsValid)
                return View(depositTypeDto);

            var documents = await FileHelper.CreateDTOListFromUploadedFilesAsync<DocumentsDTO>(documentFiles);
            await _depositTypeService.AddAsync(depositTypeDto, documents);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var depositType = await _depositTypeService.GetByIdWithDetailsAsync(id);
            if (depositType == null)
                return NotFound();

            return View(depositType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            DepositTypeDTO depositTypeDto, // Убрали параметр id
            List<IFormFile>? newDocumentFiles,
            List<Guid>? documentsToDelete,
            Guid? primaryDocumentId)
        {
            if (!ModelState.IsValid)
                return View(depositTypeDto);

            // Убрали проверку id

            await _depositTypeService.UpdateAsync(
                depositTypeDto,
                newDocumentFiles,
                documentsToDelete,
                primaryDocumentId);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _depositTypeService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var depositType = await _depositTypeService.GetByIdWithDetailsAsync(id);
            if (depositType == null)
                return NotFound();

            return View(depositType);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDocument(Guid documentId)
        {
            await _depositTypeService.DeleteDocumentAsync(documentId);
            TempData["Message"] = $"Document with ID {documentId} deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}