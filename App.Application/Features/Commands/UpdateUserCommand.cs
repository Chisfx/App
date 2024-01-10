using App.Application.Interfaces.Repositories;
using App.Application.Interfaces.Shared;
using AspNetCoreHero.Results;
using AutoMapper;
using App.Domain.Entities;
using MediatR;
using App.Domain.DTOs;
using Microsoft.Extensions.Logging;
namespace App.Application.Features.Commands
{
    /// <summary>
    /// Represents a command to update a user.
    /// </summary>
    public class UpdateUserCommand : IRequest<Result<UserModel>>
    {
        /// <summary>
        /// Gets or sets the ID of the user.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the email of the user.
        /// </summary>
        public string Email { get; set; } = default!;

        /// <summary>
        /// Gets or sets the age of the user.
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Represents the handler for the UpdateUserCommand.
        /// </summary>
        public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UserModel>>
        {
            private readonly IRepositoryAsync<User> _repository;
            private readonly IMapper _mapper;
            private IUnitOfWork _unitOfWork { get; set; }
            private readonly ICompareObject _compare;
            private readonly ILogger<UpdateUserCommand> _logger;

            /// <summary>
            /// Initializes a new instance of the UpdateUserCommandHandler class.
            /// </summary>
            public UpdateUserCommandHandler(
                IRepositoryAsync<User> repository,
                IUnitOfWork unitOfWork,
                IMapper mapper,
                ICompareObject compare,
                ILogger<UpdateUserCommand> logger)
            {
                _repository = repository;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _compare = compare;
                _logger = logger;
            }

            /// <summary>
            /// Handles the UpdateUserCommand.
            /// </summary>
            public async Task<Result<UserModel>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    UserModel result = null;
                    string msg = string.Empty;

                    await _unitOfWork.BeginTransactionAsync(cancellationToken);

                    var entity = await _repository.GetByIdAsync(request.Id);

                    if (entity == null)
                    {
                        msg = "User not found.";
                    }
                    else
                    {
                        var exist = await _repository.AnyAsync(p => p.Id != request.Id && p.Email == request.Email);
                        if (exist)
                        {
                            msg = $"Email {request.Email} already exists.";
                        }
                        else
                        {
                            if (_compare.Compare(_mapper.Map<UserModel>(entity), _mapper.Map<UserModel>(request)))
                            {
                                msg = "Must update records.";
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(msg))
                    {
                        _mapper.Map(request, entity);

                        await _repository.UpdateAsync(entity);

                        await _unitOfWork.Commit(cancellationToken);

                        if (entity.Id == 0)
                        {
                            throw new Exception("Database Error");
                        }

                        result = _mapper.Map<UserModel>(entity);
                    }

                    await _unitOfWork.CommitTransactionAsync(cancellationToken);

                    return await Result<UserModel>.SuccessAsync(result, msg);
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    _logger.LogError(ex.Message);
                    return await Result<UserModel>.FailAsync(ex.Message);
                }
            }
        }
    }
}
