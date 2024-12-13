namespace WebObjectsBLL.DTO
{
    public class PhoneDTO
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public bool IsPrimary { get; set; }
        public int PhoneTypeId { get; set; }
        public Guid AssociatedRecordId { get; set; }
    }
}
