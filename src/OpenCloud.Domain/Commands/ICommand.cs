using MediatR;

namespace OpenCloud.Domain.Commands;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}