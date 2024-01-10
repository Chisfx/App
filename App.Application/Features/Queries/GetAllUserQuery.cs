using App.Application.Interfaces.Repositories;
using AspNetCoreHero.Results;
using AutoMapper;
using Bogus;
using App.Domain.Entities;
using MediatR;
using App.Domain.DTOs;
using Microsoft.Extensions.Logging;
namespace App.Application.Features.Queries
{
    public class GetAllUserQuery : IRequest<Result<List<UserModel>>>
    {
        public bool Faker { get; set; }
        public int Top { get; set; }
        public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, Result<List<UserModel>>>
        {
            private readonly IRepositoryAsync<User> _repository;
            private readonly IMapper _mapper;
            private readonly ILogger<GetAllUserQuery> _logger;
            public GetAllUserQueryHandler(
                IRepositoryAsync<User> repository,
                IMapper mapper,
                ILogger<GetAllUserQuery> logger)
            {
                _repository = repository;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<Result<List<UserModel>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    List<UserModel> entities;
                    if (request.Faker)
                    {
                        var user = new Faker<UserModel>()
                        .RuleFor(c => c.Name, (k, a) => $"{k.Name.FirstName()} {k.Name.LastName()}")
                        .RuleFor(c => c.Email, (k, a) => k.Internet.Email(a.Name))
                        .RuleFor(c => c.Age, k => k.Random.Int(18, 60));

                        entities = user.Generate(request.Top);
                    }
                    else
                    {
                        var response = await _repository.GetAllAsync();
                        entities = _mapper.Map<List<UserModel>>(response);
                    }

                    return await Result<List<UserModel>>.SuccessAsync(entities);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return await Result<List<UserModel>>.FailAsync(ex.Message);
                }
            }
        }

    }
}
