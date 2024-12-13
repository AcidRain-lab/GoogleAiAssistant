namespace BLL.DTO.Emails
{
    public class EmailAttachmentDTO
    {
        public string FileName { get; set; }=string.Empty;
        public byte[] Content { get; set; }
        public string Base64Content { get; set; } = string.Empty;
        public string MimeType { get; set; } = string.Empty;
    }
}
