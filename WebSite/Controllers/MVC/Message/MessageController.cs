using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;

namespace WebSite.Controllers.MVC
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly MessageService _messageService;

        public MessageController(MessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task<IActionResult> Index(Guid userId)
        {
            var messages = await _messageService.GetByUserIdAsync(userId);
            ViewBag.UserId = userId;
            return PartialView("Index", messages); // Частичное представление
        }


        public IActionResult Add(Guid userId)
        {
            var message = new MessageDTO { UserId = userId };
            return View(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(MessageDTO messageDto)
        {
            if (!ModelState.IsValid)
            {
                return View(messageDto);
            }

            await _messageService.CreateAsync(messageDto);
            return RedirectToAction(nameof(Index), new { userId = messageDto.UserId });
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var message = await _messageService.GetByIdAsync(id);
            if (message == null)
                return NotFound();

            return View(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MessageDTO messageDto)
        {
            if (!ModelState.IsValid)
            {
                return View(messageDto);
            }

            await _messageService.UpdateAsync(messageDto);
            return RedirectToAction(nameof(Index), new { userId = messageDto.UserId });
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var message = await _messageService.GetByIdAsync(id);
            if (message == null)
                return NotFound();

            return View(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, Guid userId)
        {
            await _messageService.DeleteAsync(id);
            return RedirectToAction(nameof(Index), new { userId });
        }
    }
}
