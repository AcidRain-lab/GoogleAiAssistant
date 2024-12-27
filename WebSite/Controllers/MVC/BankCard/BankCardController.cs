using Microsoft.AspNetCore.Mvc;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;

public class BankCardController : Controller
{
    private readonly BankCardService _bankCardService;
    private readonly CardTypesService _cardTypeService;

    public BankCardController(BankCardService bankCardService, CardTypesService cardTypeService)
    {
        _bankCardService = bankCardService;
        _cardTypeService = cardTypeService;
    }

    public async Task<IActionResult> Index(Guid clientId)
    {
        if (clientId == Guid.Empty)
            return BadRequest("Client ID is required.");

        var cards = await _bankCardService.GetByClientIdAsync(clientId);
        ViewBag.ClientId = clientId;
        return View(cards);
    }

    [HttpGet]
    public async Task<IActionResult> Add(Guid clientId)
    {
        if (clientId == Guid.Empty)
            return BadRequest("Client ID is required.");

        var cardTypes = await _cardTypeService.GetAllWithAvatarsAsync();
        ViewBag.CardTypes = cardTypes;
        ViewBag.ClientId = clientId;

        var newCard = new BankCardDTO
        {
            CardNumber = GenerateCardNumber(),
            CardHolderName = "Default Name",
            ExpirationDate = DateTime.Now.AddYears(3)
        };

        return View(newCard);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(Guid clientId, Guid cardTypeId, string cardHolderName)
    {
        if (clientId == Guid.Empty)
            return BadRequest("Client ID is required.");

        await _bankCardService.CreateAsync(clientId, cardTypeId, cardHolderName);
        return RedirectToAction(nameof(Index), new { clientId });
    }

    private string GenerateCardNumber()
    {
        return $"4000{new Random().Next(100000000, 999999999)}";
    }
}
