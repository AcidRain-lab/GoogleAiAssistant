using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;

namespace WebSite.Controllers.MVC.TermsAndRules
{
    [Authorize]
    [Controller]
    public class TermsAndRulesController : Controller
    {
        private readonly TermsAndRulesService _termsAndRulesService;

        public TermsAndRulesController(TermsAndRulesService termsAndRulesService)
        {
            _termsAndRulesService = termsAndRulesService;
        }

        // Метод для отображения древовидной структуры условий и правил
        public async Task<IActionResult> Index()
        {
            var termsAndRules = await _termsAndRulesService.GetTermsAndRulesTreeAsync();
            return View(termsAndRules); // Передача данных в View
        }

        // API-метод для получения структуры в JSON формате
        [HttpGet("api/terms-and-rules/tree")]
        public async Task<IActionResult> GetTermsAndRulesTree()
        {
            var termsAndRules = await _termsAndRulesService.GetTermsAndRulesTreeAsync();
            return Json(termsAndRules); // Возвращение данных в формате JSON
        }
    }
}
