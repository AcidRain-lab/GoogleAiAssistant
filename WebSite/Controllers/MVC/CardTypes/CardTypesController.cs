using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;
using MediaLib.DTO;

namespace WebSite.Controllers.MVC
{
    [Authorize]
    [Route("CardTypes")]
    public class CardTypesController : Controller
    {
        private readonly CardTypesService _cardTypesService;
        private readonly PaymentSystemService _paymentSystemService;

        public CardTypesController(CardTypesService cardTypesService, PaymentSystemService paymentSystemService)
        {
            _cardTypesService = cardTypesService;
            _paymentSystemService = paymentSystemService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var cardTypes = await _cardTypesService.GetAllAsync();
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
        public async Task<IActionResult> Add(CardTypeDTO cardTypeDto, IFormFile? avatarFile)
        {
            //if (!ModelState.IsValid)
            //{
            //    ViewBag.PaymentSystemTypes = new SelectList(await _paymentSystemService.GetAllAsync(), "Id", "Name");
            //    return View(cardTypeDto);
            //}

            AvatarDTO? avatar = null;
            if (avatarFile != null)
            {
                avatar = new AvatarDTO
                {
                    ImgName = avatarFile.FileName,
                    Base64Image = Convert.ToBase64String(await GetFileBytesAsync(avatarFile))
                };
            }

            await _cardTypesService.AddAsync(cardTypeDto, avatar);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var cardType = await _cardTypesService.GetByIdAsync(id);
            if (cardType == null)
                return NotFound();

            ViewBag.PaymentSystemTypes = new SelectList(await _paymentSystemService.GetAllAsync(), "Id", "Name");
            return View(cardType);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CardTypeDTO cardTypeDto, IFormFile? avatarFile)
        {
            //if (!ModelState.IsValid)
            //{
            //    ViewBag.PaymentSystemTypes = new SelectList(await _paymentSystemService.GetAllAsync(), "Id", "Name");
            //    return View(cardTypeDto);
            //}

            AvatarDTO? avatar = null;
            if (avatarFile != null)
            {
                avatar = new AvatarDTO
                {
                    ImgName = avatarFile.FileName,
                    Base64Image = Convert.ToBase64String(await GetFileBytesAsync(avatarFile))
                };
            }

            await _cardTypesService.UpdateAsync(cardTypeDto, avatar);
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
            var cardType = await _cardTypesService.GetByIdAsync(id);
            if (cardType == null)
                return NotFound();

            return View(cardType);
        }

        private async Task<byte[]> GetFileBytesAsync(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
