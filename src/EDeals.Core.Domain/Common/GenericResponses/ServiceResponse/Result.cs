using EDeals.Core.Domain.Common.GenericResponses.BaseResponses;

namespace EDeals.Core.Domain.Common.GenericResponses.ServiceResponse
{
    public abstract class Result
    {
        public virtual ResultResponse Ok() =>
            new(ResultCode.Ok);
        
        public virtual ResultResponse<T> Ok<T>() =>
            new(ResultCode.Ok);

        public virtual ResultResponse<T> Ok<T>(T data) =>
            new(ResultCode.Ok, data);

        public virtual ResultResponse Unauthorized() =>
            new(ResultCode.Unauthorized);

        public virtual ResultResponse<T> Unauthorized<T>(T data) =>
            new(ResultCode.Unauthorized, data);
        
        public virtual ResultResponse Unauthorized(params ResponseError[] errors) =>
            new(ResultCode.Unauthorized, errors);

        public virtual ResultResponse<T> Unauthorized<T>(params ResponseError[] errors) =>
            new(ResultCode.Unauthorized, errors);
        
        public virtual ResultResponse Forbidden() =>
            new(ResultCode.Forbidden);

        public virtual ResultResponse<T> Forbidden<T>(T data) =>
            new(ResultCode.Forbidden, data);

        public virtual ResultResponse Forbidden(params ResponseError[] errors) =>
            new(ResultCode.Forbidden, errors);

        public virtual ResultResponse<T> Forbidden<T>(params ResponseError[] errors) =>
            new(ResultCode.Forbidden, errors);

        public virtual ResultResponse BadRequest() =>
            new(ResultCode.BadRequest);

        public virtual ResultResponse<T> BadRequest<T>(T data) =>
            new(ResultCode.BadRequest, data);

        public virtual ResultResponse BadRequest(params ResponseError[] errors) =>
            new(ResultCode.BadRequest, errors);

        public virtual ResultResponse<T> BadRequest<T>(params ResponseError[] errors) =>
            new(ResultCode.BadRequest, errors);

        public virtual ResultResponse NotFound() =>
            new(ResultCode.NotFound);

        public virtual ResultResponse<T> NotFound<T>(T data) =>
            new(ResultCode.NotFound, data);

        public virtual ResultResponse NotFound(params ResponseError[] errors) =>
            new(ResultCode.NotFound, errors);

        public virtual ResultResponse<T> NotFound<T>(params ResponseError[] errors) =>
            new(ResultCode.NotFound, errors);

        public virtual ResultResponse InternalError() =>
            new(ResultCode.InternalError);

        public virtual ResultResponse<T> InternalError<T>(T data) =>
            new(ResultCode.InternalError, data);

        public virtual ResultResponse InternalError(params ResponseError[] errors) =>
            new(ResultCode.InternalError, errors);

        public virtual ResultResponse<T> InternalError<T>(params ResponseError[] errors) =>
            new(ResultCode.InternalError, errors);
    }
}
