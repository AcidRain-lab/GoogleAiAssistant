namespace BLL.DTO.Leads
{
    public class LeadListForDropdownDTO
    {
        public Guid LeadId { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
