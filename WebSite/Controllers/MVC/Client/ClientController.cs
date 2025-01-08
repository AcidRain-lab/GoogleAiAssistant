using MediaLib.DTO;
using MediaLib.Helpers;
using MediaLib.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;

[Authorize]
public class ClientController : Controller
{
    private readonly ClientService _clientService;
    private readonly AvatarService _avatarService;

    public ClientController(ClientService clientService, AvatarService avatarService)
    {
        _clientService = clientService;
        _avatarService = avatarService;
    }

    public async Task<IActionResult> Index()
    {
        var clients = await _clientService.GetAllAsync();
        return View(clients);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var client = await _clientService.GetDetailByIdAsync(id);
            if (client == null)
            {
                TempData["Error"] = "Client not found.";
                return RedirectToAction(nameof(Index));
            }

            client.Avatar = await _avatarService.GetAvatarAsync(client.Id);
            return View(client);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"An unexpected error occurred: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    public IActionResult Add()
    {
        return View(new ClientDetailDTO());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(ClientDetailDTO clientDetailDto)
    {
        if (!ModelState.IsValid)
        {
            return View(clientDetailDto);
        }

        try
        {
            await _clientService.CreateAsync(clientDetailDto);
            TempData["Message"] = "Client created successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
            return View(clientDetailDto);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            var client = await _clientService.GetDetailByIdAsync(id);
            if (client == null)
            {
                TempData["Error"] = "Client not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"An unexpected error occurred: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ClientDetailDTO clientDetailDto)
    {
        if (!ModelState.IsValid)
        {
            return View(clientDetailDto);
        }

        try
        {
            await _clientService.UpdateAsync(clientDetailDto);
            TempData["Message"] = "Client updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException)
        {
            TempData["Error"] = "Client not found.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
            return View(clientDetailDto);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _clientService.DeleteAsync(id);
            TempData["Message"] = "Client deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"An unexpected error occurred: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> UpdateAvatar(Guid clientId, IFormFile? avatarFile)
    //{
    //    var avatar = avatarFile != null
    //        ? await FileHelper.CreateDTOFromUploadedFileAsync<AvatarDTO>(avatarFile)
    //        : null;

    //    await _clientService.AvatarUpdateAsync(clientId, avatar);

    //    TempData["Message"] = "Avatar updated successfully.";
    //    return RedirectToAction(nameof(Details), new { id = clientId });
    //}

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateAvatar(Guid associatedRecordId, IFormFile? avatarFile)
    {
        if (avatarFile != null)
        {
            var avatarDto = await FileHelper.CreateDTOFromUploadedFileAsync<AvatarDTO>(avatarFile);
            avatarDto.AssociatedRecordId = associatedRecordId;
            avatarDto.ObjectTypeId = (int)MediaLib.ObjectType.Client; // Замените на нужный тип

            await _avatarService.SetAvatarAsync(avatarDto);
        }
        else
        {
            await _avatarService.RemoveAvatarAsync(associatedRecordId);
        }

        return RedirectToAction("Details", new { id = associatedRecordId });
    }

}
