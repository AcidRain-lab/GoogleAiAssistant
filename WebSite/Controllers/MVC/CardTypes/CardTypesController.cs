using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;
using MediaLib.DTO;
using MediaLib.Helpers;

namespace WebSite.Controllers.MVC
{
    [Authorize]
    [Route("CardTypes")]
    public class CardTypesController : Controller
    {
        private readonly CardTypesService _cardTypesService;
        private readonly PaymentSystemService _paymentSystemService;

        public CardTypesController(
            CardTypesService cardTypesService,
            PaymentSystemService paymentSystemService)
        {
            _cardTypesService = cardTypesService;
            _paymentSystemService = paymentSystemService;
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
            var avatar = await FileHelper.CreateDTOFromUploadedFileAsync<AvatarDTO>(avatarFile);
            var mediaDTOs = await FileHelper.CreateDTOListFromUploadedFilesAsync<MediaDataDTO>(mediaFiles);
            var documentDTOs = await FileHelper.CreateDTOListFromUploadedFilesAsync<DocumentsDTO>(documentFiles);

            await _cardTypesService.AddAsync(cardTypeDto, avatar, mediaDTOs, documentDTOs);
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
            List<IFormFile>? documentFiles)
        {
            var avatar = await FileHelper.CreateDTOFromUploadedFileAsync<AvatarDTO>(avatarFile);
            var mediaDTOs = await FileHelper.CreateDTOListFromUploadedFilesAsync<MediaDataDTO>(mediaFiles);
            var documentDTOs = await FileHelper.CreateDTOListFromUploadedFilesAsync<DocumentsDTO>(documentFiles);

            await _cardTypesService.UpdateAsync(cardTypeDto, avatar, mediaDTOs, documentDTOs);
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
    }
}
