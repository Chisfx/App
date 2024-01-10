using App.Application.Interfaces.Repositories;
using AspNetCoreHero.Results;
using AutoMapper;
using App.Domain.Entities;
using MediatR;
using App.Domain.DTOs;
using App.Application.Features.Commands;
using Microsoft.Extensions.Logging;
namespace App.Application.Features.Queries
{
    public class GetUserByIdQuery : IRequest<Result<UserModel>>
    {
        public int Id { get; set; }
        public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserModel>>
        {
            private readonly IRepositoryAsync<User> _repository;
            private readonly IMapper _mapper;
            private readonly ILogger<GetUserByIdQuery> _logger;
            public GetUserByIdQueryHandler(
                IRepositoryAsync<User> repository,
                IMapper mapper,
                ILogger<GetUserByIdQuery> logger)
            {
                _repository = repository;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<Result<UserModel>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    UserModel result = null;
                    string msg = string.Empty;

                    var entity = await _repository.GetByIdAsync(request.Id);

                    if (entity == null)
                    {
                        msg = "User not found.";
                    }
                    else
                    {
                        result = _mapper.Map<UserModel>(entity);
                    }

                    return await Result<UserModel>.SuccessAsync(result, msg);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return await Result<UserModel>.FailAsync(ex.Message);
                }
            }
        }

    }
}
