using BLL.DTO.Contacts;
using BLL.DTO.Trades;
using System.ComponentModel.DataAnnotations;
namespace BLL.DTO.Templates
{
    public class TemplatesInputDTO
    {
        public Guid? Id { get; set; }
        [Required(ErrorMessage = "Temalate name is required.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Temalate text is required.")]
        public string Text { get; set; } = string.Empty;
        public bool IsNewRecord { get; set; }
        //[Range(1, int.MaxValue, ErrorMessage = "Please select trade for template.")]
        [Required( ErrorMessage = "Please select trade for template.")]
        public Guid TradeId { get; set; }
        public Guid? UserId { get; set; }
        public string? TradeName { get; set; } = string.Empty;
        public List<TradeListForDropdownDTO> TradeList { get; set; } = new() { new TradeListForDropdownDTO { TradeId = Guid.Empty, Name = "Select Trade" } };
    }
}
