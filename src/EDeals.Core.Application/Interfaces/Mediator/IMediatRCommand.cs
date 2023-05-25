using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;
using MediatR;

namespace EDeals.Core.Application.Interfaces.Mediator
{
    public interface IMediatRCommand : IRequest<ResultResponse>
    {
    }

    public interface IMediatRCommand<TResponse> : IRequest<ResultResponse<TResponse>>
    {
    }
}
