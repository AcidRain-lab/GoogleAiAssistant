namespace WebObjectsBLL.DTO
{
    public class ClientDTO
    {
        public Guid Id { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public string? Email { get; set; }

        public Guid? UserId { get; set; }

        public string? Phone { get; set; }

        public bool? IsActive { get; set; }

        public bool IsIndividual { get; set; } // Определяет тип клиента

        public string? Name { get; set; } // Полное имя или название компании
    }
}
