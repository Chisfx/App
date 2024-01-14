using App.Application.Interfaces.Repositories;
using AspNetCoreHero.Results;
using AutoMapper;
using Bogus;
using App.Domain.Entities;
using MediatR;
using App.Domain.DTOs;
using Microsoft.Extensions.Logging;
using Bogus.Extensions;
namespace App.Application.Features.Queries
{
    /// <summary>
    /// Represents a query to get all users.
    /// </summary>
    public class GetAllUserQuery : IRequest<Result<List<UserModel>>>
    {
        /// <summary>
        /// Gets or sets a value indicating whether to use Faker to generate fake user data.
        /// </summary>
        public bool Faker { get; set; }

        /// <summary>
        /// Gets or sets the number of users to retrieve.
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// Represents a handler for the GetAllUserQuery.
        /// </summary>
        public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, Result<List<UserModel>>>
        {
            private readonly IRepositoryAsync<User> _repository;
            private readonly IMapper _mapper;
            private readonly ILogger<GetAllUserQuery> _logger;

            /// <summary>
            /// Initializes a new instance of the <see cref="GetAllUserQueryHandler"/> class.
            /// </summary>
            /// <param name="repository">The user repository.</param>
            /// <param name="mapper">The mapper.</param>
            /// <param name="logger">The logger.</param>
            public GetAllUserQueryHandler(
                IRepositoryAsync<User> repository,
                IMapper mapper,
                ILogger<GetAllUserQuery> logger)
            {
                _repository = repository;
                _mapper = mapper;
                _logger = logger;
            }

            /// <summary>
            /// Handles the GetAllUserQuery request.
            /// </summary>
            /// <param name="request">The GetAllUserQuery request.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            /// <returns>A Result object containing the list of users.</returns>
            public async Task<Result<List<UserModel>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    List<UserModel> entities;
                    if (request.Faker)
                    {
                        var user = new Faker<UserModel>()
                            .RuleFor(c => c.Name, (k, a) => k.Name.FullName().ClampLength(min: 3, max: 50))
                            .RuleFor(c => c.Email, (k, a) => k.Internet.Email(a.Name).ClampLength(min: 5))
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
