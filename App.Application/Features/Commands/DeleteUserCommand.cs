using App.Application.Interfaces.Repositories;
using AspNetCoreHero.Results;
using App.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.Commands
{
    /// <summary>
    /// Represents a command to delete a user.
    /// </summary>
    public class DeleteUserCommand : IRequest<Result<bool>>
    {
        /// <summary>
        /// Gets or sets the ID of the user to delete.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents the handler for the DeleteUserCommand.
        /// </summary>
        public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result<bool>>
        {
            private readonly IRepositoryAsync<User> _repository;
            private IUnitOfWork _unitOfWork { get; set; }
            private readonly ILogger<DeleteUserCommand> _logger;

            /// <summary>
            /// Initializes a new instance of the DeleteUserCommandHandler class.
            /// </summary>
            /// <param name="repository">The repository used to access user data.</param>
            /// <param name="unitOfWork">The unit of work for managing transactions.</param>
            /// <param name="logger">The logger used for logging.</param>
            public DeleteUserCommandHandler(
                IRepositoryAsync<User> repository,
                IUnitOfWork unitOfWork,
                ILogger<DeleteUserCommand> logger)
            {
                _repository = repository;
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            /// <summary>
            /// Handles the DeleteUserCommand.
            /// </summary>
            /// <param name="request">The DeleteUserCommand to handle.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            /// <returns>A Result indicating whether the user was deleted successfully.</returns>
            public async Task<Result<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    string msg = string.Empty;

                    await _unitOfWork.BeginTransactionAsync(cancellationToken);

                    var entity = await _repository.GetByIdAsync(request.Id);

                    if (entity == null)
                    {
                        msg = "User not found.";
                    }

                    if (string.IsNullOrEmpty(msg))
                    {
                        await _repository.DeleteAsync(entity);

                        await _unitOfWork.Commit(cancellationToken);
                    }

                    await _unitOfWork.CommitTransactionAsync(cancellationToken);

                    return await Result<bool>.SuccessAsync(true, msg);
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    _logger.LogError(ex.Message);
                    return await Result<bool>.FailAsync(ex.Message);
                }
            }
        }
    }
}
