namespace JTM.DTO.ExceptionResponse
{
    public sealed record ExceptionResponse
    {
        public string Title { get; init; }
        public int StatusCode { get; init; }
        public IEnumerable<ExceptionError> Errors { get; init; }

        public ExceptionResponse(string title, int statusCode, IEnumerable<ExceptionError> errors)
        {
            Title = title;
            StatusCode = statusCode;
            Errors = errors;
        }
    }
}
