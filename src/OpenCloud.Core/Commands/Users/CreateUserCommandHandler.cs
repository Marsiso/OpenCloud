using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OpenCloud.Data;
using OpenCloud.Domain.Commands;
using OpenCloud.Domain.Models;

namespace OpenCloud.Core.Commands.Users;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Unit>
{
	private readonly DataContext _databaseContext;
	private readonly ILogger<CreateUserCommandHandler> _logger;
	private readonly IMapper _mapper;

	public CreateUserCommandHandler(DataContext databaseContext, IMapper mapper, ILogger<CreateUserCommandHandler> logger)
	{
		_databaseContext = databaseContext;
		_mapper = mapper;
		_logger = logger;
	}

	public Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		var userToCreate = _mapper.Map<User>(request);

		_databaseContext.Users.Add(userToCreate);

		cancellationToken.ThrowIfCancellationRequested();

		_databaseContext.SaveChanges();

		return Unit.Task;
	}
}