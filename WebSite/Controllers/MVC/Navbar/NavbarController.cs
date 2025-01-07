using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using WebObjectsBLL.Services;

namespace WebSite.Controllers.MVC
{
    public class NavbarController : Controller
    {
        private readonly CardTypesService _cardTypeService;
        private readonly DepositTypeService _depositTypeService;
        private readonly CreditTypeService _creditTypeService;

        public NavbarController(CardTypesService cardTypeService, DepositTypeService depositTypeService, CreditTypeService creditTypeService)
        {
            _cardTypeService = cardTypeService;
            _depositTypeService = depositTypeService;
            _creditTypeService = creditTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> Navbar()
        {
            // Получение данных для выпадающего меню
            ViewBag.CardTypes = await _cardTypeService.GetAllWithAvatarsAsync();
            ViewBag.Deposits = await _depositTypeService.GetAllWithAvatarsAsync();
            ViewBag.CreditTypes = await _creditTypeService.GetAllAsync();

            // Убедитесь, что данные действительно возвращаются
            if (ViewBag.CardTypes == null || ViewBag.Deposits == null || ViewBag.CreditTypes == null)
            {
                throw new Exception("One or more dropdown data sources are null.");
            }

            return PartialView("_NavBar_user");
        }



    }
}
