using App.Application.Interfaces.Repositories;
using AspNetCoreHero.Results;
using App.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.Commands
{
    public class DeleteUserCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
        public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result<bool>>
        {
            private readonly IRepositoryAsync<User> _repository;
            private IUnitOfWork _unitOfWork { get; set; }
            private readonly ILogger<DeleteUserCommand> _logger;
            public DeleteUserCommandHandler(
                IRepositoryAsync<User> repository,
                IUnitOfWork unitOfWork,
                ILogger<DeleteUserCommand> logger)
            {
                _repository = repository;
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

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
