
using BLL.DTO.Templates;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Trades
{
    public class TradesInputDTO
    {
        public Guid? Id { get; set; }
        [Required(ErrorMessage = "Trade name is required.")]
        public string Name { get; set; } = string.Empty;
        public bool IsNewRecord { get; set; }
        public List<TemplateDTO> TemplatesList { get; set; } = new();
    }
}
