using App.Application.Interfaces.Repositories;
using AspNetCoreHero.Results;
using AutoMapper;
using App.Domain.Entities;
using MediatR;
using App.Application.Features.Queries;
using Microsoft.Extensions.Logging;
using App.Domain.DTOs;

namespace App.Application.Features.Commands
{
    /// <summary>
    /// Represents a command to create a user.
    /// </summary>
    public class CreateUserCommand : IRequest<Result<UserModel>>
    {
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
        /// Represents the handler for the CreateUserCommand.
        /// </summary>
        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserModel>>
        {
            private readonly IRepositoryAsync<User> _repository;
            private readonly IMapper _mapper;
            private IUnitOfWork _unitOfWork { get; set; }
            private readonly ILogger<CreateUserCommand> _logger;

            /// <summary>
            /// Initializes a new instance of the <see cref="CreateUserCommandHandler"/> class.
            /// </summary>
            /// <param name="repository">The user repository.</param>
            /// <param name="unitOfWork">The unit of work.</param>
            /// <param name="mapper">The mapper.</param>
            /// <param name="logger">The logger.</param>
            public CreateUserCommandHandler(
                IRepositoryAsync<User> repository,
                IUnitOfWork unitOfWork,
                IMapper mapper,
                ILogger<CreateUserCommand> logger)
            {
                _repository = repository;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _logger = logger;
            }

            /// <summary>
            /// Handles the CreateUserCommand.
            /// </summary>
            /// <param name="request">The CreateUserCommand request.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            /// <returns>The result of the CreateUserCommand.</returns>
            public async Task<Result<UserModel>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    UserModel result = null;
                    string msg = string.Empty;

                    await _unitOfWork.BeginTransactionAsync(cancellationToken);

                    var exist = await _repository.AnyAsync(p => p.Email == request.Email);
                    if (exist)
                    {
                        msg = $"Email {request.Email} already exists.";
                    }
                    else
                    {
                        var entity = _mapper.Map<User>(request);

                        await _repository.AddAsync(entity);

                        await _unitOfWork.Commit(cancellationToken);

                        if (entity.Id == 0) throw new Exception("Database Error");

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
