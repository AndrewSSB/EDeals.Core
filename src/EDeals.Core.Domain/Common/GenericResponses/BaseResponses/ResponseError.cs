using EDeals.Core.Domain.Common.ErrorMessages;

namespace EDeals.Core.Domain.Common.GenericResponses.BaseResponses
{
    public class ResponseError
    {
        public ErrorCodes Code { get; set; }
        public ResponseErrorSeverity SeverityError { get; set; }
        public string Description { get; set; }
        public DateTime TimeStamp { get; set; }
        public string? Path { get; set; }

        public ResponseError(ErrorCodes code, ResponseErrorSeverity severityError, string description, string? path = null, DateTime? timeStamp = null)
        {
            Code = code;
            SeverityError = severityError;
            Description = description;
            TimeStamp = timeStamp ?? DateTime.UtcNow;
            Path = path ?? string.Empty;
        }
    }
}
