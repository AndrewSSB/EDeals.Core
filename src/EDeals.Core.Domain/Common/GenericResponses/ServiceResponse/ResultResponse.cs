using EDeals.Core.Domain.Common.GenericResponses.BaseResponses;

namespace EDeals.Core.Domain.Common.GenericResponses.ServiceResponse
{
    public class ResultResponse
    {
        public ResultCode? Result { get ; private set; }
        public List<ResponseError>? Errors { get; private set; }

        public ResultResponse()
        {
            Result = ResultCode.Ok;
            Errors = null;
        }
        public ResultResponse(ResultCode result)
        {
            Result = result;
            Errors = null;
        }

        public ResultResponse(ResultCode result, ResponseError errors)
        {
            Result = result;
            Errors = new List<ResponseError> { errors };
        }
        
        public ResultResponse(ResultCode result, ResponseError[]? errors)
        {
            Result = result;
            Errors = errors?.ToList();
        }
    }

    public class ResultResponse<TResponse> : ResultResponse
    {
        public TResponse? ResponseData { get; private set; }

        public ResultResponse()
            : base()
        {
        }

        public ResultResponse(ResultCode result, TResponse data)
            : base(result)
        {
            ResponseData = data;
        }

        public ResultResponse(ResultCode result, ResponseError errors) 
            : base(result, errors) 
        {
            ResponseData = default;
        }
        
        public ResultResponse(ResultCode result, ResponseError[]? errors = null) 
            : base(result, errors) 
        {
            ResponseData = default;
        }

        public ResultResponse(ResultCode result, TResponse data, ResponseError[]? errors = null)
            : base(result, errors)
        {
            ResponseData = data;
        }
    }
}
