namespace JTM.DTO.Account.RegisterUser
{
    public sealed record MessageDto
    {
        public string ReceiverName { get; init; }
        public string ReceiverEmail { get; init; }
        public string Url { get; init; }

        public MessageDto(string receiverName, string receiverEmail, string url)
        {
            ReceiverName = receiverName;
            ReceiverEmail = receiverEmail;
            Url = url;
        }
    }
}
