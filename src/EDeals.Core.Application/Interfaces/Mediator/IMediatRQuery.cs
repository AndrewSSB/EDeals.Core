using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;
using MediatR;

namespace EDeals.Core.Application.Interfaces.Mediator
{
    public interface IMediatRQuery : IRequest<ResultResponse>
    {
    }
    
    public interface IMediatRQuery<TResponse> : IRequest<ResultResponse<TResponse>>
    {
    }
}
