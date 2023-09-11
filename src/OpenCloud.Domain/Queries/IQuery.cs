using MediatR;

namespace OpenCloud.Domain.Queries;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}