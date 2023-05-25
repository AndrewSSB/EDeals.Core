namespace EDeals.Core.Domain.Common.GenericResponses.BaseResponses
{
    public class BaseResponseError
    {
        public List<ResponseError> Errors { get; set; }

        public BaseResponseError()
        {
            Errors = new List<ResponseError>();
        }

        public BaseResponseError(List<ResponseError> errors)
        {
            Errors = errors;
        }

        public BaseResponseError(params ResponseError[] errors)
            : this(errors.ToList())
        {
        }
    }
}
