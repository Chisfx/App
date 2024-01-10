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
    public class CreateUserCommand : IRequest<Result<UserModel>>
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public int Age { get; set; }

        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserModel>>
        {
            private readonly IRepositoryAsync<User> _repository;
            private readonly IMapper _mapper;
            private IUnitOfWork _unitOfWork { get; set; }
            private readonly ILogger<CreateUserCommand> _logger;
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
                        msg = $"Email {request.Email} already exist.";
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
