namespace WebObjectsBLL.DTO
{
    public class DocumentsDataDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public byte[] Content { get; set; }
        public Guid AssociatedRecordId { get; set; }
        public bool IsPrime { get; set; }
        public int ObjectTypeId { get; set; }
        public Guid? OwnerId { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
