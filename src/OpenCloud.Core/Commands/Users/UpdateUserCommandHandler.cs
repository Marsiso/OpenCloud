using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenCloud.Data;
using OpenCloud.Domain.Commands;
using OpenCloud.Domain.Exceptions;
using OpenCloud.Domain.Models;

namespace OpenCloud.Core.Commands.Users;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, Unit>
{
	private readonly DataContext _databaseContext;
	private readonly ILogger<UpdateUserCommandHandler> _logger;
	private readonly IMapper _mapper;

	public UpdateUserCommandHandler(DataContext databaseContext, IMapper mapper, ILogger<UpdateUserCommandHandler> logger)
	{
		_databaseContext = databaseContext;
		_logger = logger;
		_mapper = mapper;
	}

	public Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		var originalUser = _databaseContext.Users.AsTracking().SingleOrDefault(user => user.UserID == request.UserID);

		cancellationToken.ThrowIfCancellationRequested();

		if (originalUser is null) throw new EntityNotFoundException(request.UserID, nameof(User));

		_mapper.Map(request, originalUser);

		_databaseContext.SaveChanges();

		return Unit.Task;
	}
}