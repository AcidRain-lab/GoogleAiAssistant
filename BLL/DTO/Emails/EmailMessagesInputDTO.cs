namespace BLL.DTO.Emails
{
    public class EmailMessagesInputDTO
    {
        public List<MailMessagesDTO> Inbox { get; set; } = new();
        public List<MailMessagesDTO> Sent { get; set; } = new();
        public List<MailMessagesDTO> Draft { get; set; } = new();
    }
}
