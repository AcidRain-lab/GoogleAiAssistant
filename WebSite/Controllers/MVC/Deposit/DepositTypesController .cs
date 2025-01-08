using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediaLib.DTO;
using MediaLib.Helpers;
using MediaLib.Services;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;
using System.Security.Claims;

namespace WebSite.Controllers.MVC.DepositType
{
    [Authorize]
    [Route("DepositTypes")]
    public class DepositTypesController : Controller
    {
        private readonly DepositTypeService _depositTypeService;
        private readonly DepositTermService _depositTermService;
        private readonly DocumentService _documentService;

        public DepositTypesController(
            DepositTypeService depositTypeService,
            DepositTermService depositTermService,
            DocumentService documentService)
        {
            _depositTypeService = depositTypeService;
            _depositTermService = depositTermService;
            _documentService = documentService;
        }

        private Guid GetOwnerId()
        {
            return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var depositTypes = await _depositTypeService.GetAllWithDetailsAsync();
            return View(depositTypes);
        }

        [HttpGet("Add")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost("Add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(
            DepositTypeDetailDTO depositTypeDto,
            IFormFile? avatarFile,
            List<IFormFile>? mediaFiles,
            List<IFormFile>? documentFiles)
        {
            if (!ModelState.IsValid)
                return View(depositTypeDto);

            var ownerId = GetOwnerId();

            var avatar = await FileHelper.CreateDTOFromUploadedFileAsync<AvatarDTO>(avatarFile);
            var mediaDTOs = await FileHelper.CreateDTOListFromUploadedFilesAsync<MediaDataDTO>(mediaFiles);
            var documentDTOs = await FileHelper.CreateDTOListFromUploadedFilesAsync<DocumentsDTO>(documentFiles);

            await _depositTypeService.AddAsync(depositTypeDto, avatar, mediaDTOs, documentDTOs, ownerId);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var depositType = await _depositTypeService.GetByIdWithDetailsAsync(id);
            if (depositType == null)
                return NotFound();

            var terms = await _depositTermService.GetByDepositTypeIdAsync(id);
            ViewBag.DepositTerms = terms.OrderBy(t => t.TermMonths).ToList();
            return View(depositType);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
     DepositTypeDetailDTO depositTypeDto,
     IFormFile? avatarFile,
     List<IFormFile>? mediaFiles,
     List<IFormFile>? documentFiles,
     Guid? primaryMediaId,
     List<Guid>? mediaToDelete,
     Guid? primaryDocumentId,
     List<Guid>? documentsToDelete)
        {
            if (!ModelState.IsValid)
                return View(depositTypeDto);

            var ownerId = GetOwnerId();

            var avatar = await FileHelper.CreateDTOFromUploadedFileAsync<AvatarDTO>(avatarFile);

            // Обновляем только тип депозита и связанные медиа и документы
            await _depositTypeService.UpdateAsync(
                depositTypeDto,
                avatar,
                mediaFiles,
                documentFiles,
                primaryMediaId,
                mediaToDelete,
                primaryDocumentId,
                documentsToDelete,
                ownerId);

            TempData["Message"] = "Deposit Type updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _depositTypeService.DeleteAsync(id);
            TempData["Message"] = $"Deposit Type with ID {id} deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var depositType = await _depositTypeService.GetByIdWithDetailsAsync(id);
            if (depositType == null)
                return NotFound();

            return View(depositType);
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
