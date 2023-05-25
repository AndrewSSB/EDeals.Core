using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;
using MediatR;

namespace EDeals.Core.Application.Interfaces.Mediator
{
    public interface IMediatRCommandHandler<TRequest> : IRequestHandler<TRequest, ResultResponse>
        where TRequest : IMediatRCommand
    {
    }

    public interface IMediatRQueryHandler<TRequest> : IRequestHandler<TRequest, ResultResponse>
        where TRequest : IMediatRQuery
    {
    }

    public interface IMediatRCommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, ResultResponse<TResponse>>
        where TRequest : IMediatRCommand<TResponse>
    {
    }

    public interface IMediatRQueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, ResultResponse<TResponse>>
        where TRequest : IMediatRQuery<TResponse>
    {
    }
}
