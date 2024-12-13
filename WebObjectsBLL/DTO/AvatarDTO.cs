namespace WebObjectsBLL.DTO
{
    public class AvatarDTO
    {
        public Guid AssociatedRecordId { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public byte[] Content { get; set; }
        public int ObjectTypeId { get; set; }
    }
}
