namespace JTM.DTO.ExceptionResponse
{
    public sealed record ExceptionError
    {
        public string ErrorMessage { get; init; }
        public string? PropertyName { get; init; }

        public ExceptionError(string errorMessage, string? propertyName = null)
        {
            ErrorMessage = errorMessage;
            PropertyName = propertyName;
        }
    }
}
