namespace WebObjectsBLL.DTO
{
    public class ClientDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; } // Полное имя или название компании
        public bool IsFop { get; set; } // ФОП или юридическое лицо
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; }
    }

}
