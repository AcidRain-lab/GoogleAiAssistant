using BLL.DTO.Calendars;
using DAL.Models;

namespace BLL.DTO.Templates
{
    public class TemplateDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public Guid TradeId { get; set; }

        public string Text { get; set; } = null!;

        public Guid? OwnerId { get; set; }
        public string TradeName { get; set; } = string.Empty;
        public DateTime? CreatedDateTime { get; set; }

        //public virtual ICollection<EstimateTamplate> EstimateTamplates { get; set; } = new List<EstimateTamplate>();

        public virtual Trade Trade { get; set; } = null!;
    }
}
