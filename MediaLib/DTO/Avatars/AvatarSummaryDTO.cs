namespace MediaLib.DTO.Avatars
{
    public class AvatarSummaryDTO
    {
        public Guid AssociatedRecordId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Extension { get; set; } = string.Empty;

        public byte[] Content { get; set; } = default!;
    }
}
