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
    /// <summary>
    /// Represents a query to get a user by their ID.
    /// </summary>
    public class GetUserByIdQuery : IRequest<Result<UserModel>>
    {
        /// <summary>
        /// The ID of the user.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents the handler for the GetUserByIdQuery.
        /// </summary>
        public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserModel>>
        {
            private readonly IRepositoryAsync<User> _repository;
            private readonly IMapper _mapper;
            private readonly ILogger<GetUserByIdQuery> _logger;

            /// <summary>
            /// Initializes a new instance of the GetUserByIdQueryHandler class.
            /// </summary>
            /// <param name="repository">The user repository.</param>
            /// <param name="mapper">The mapper.</param>
            /// <param name="logger">The logger.</param>
            public GetUserByIdQueryHandler(
                IRepositoryAsync<User> repository,
                IMapper mapper,
                ILogger<GetUserByIdQuery> logger)
            {
                _repository = repository;
                _mapper = mapper;
                _logger = logger;
            }

            /// <summary>
            /// Handles the GetUserByIdQuery request.
            /// </summary>
            /// <param name="request">The GetUserByIdQuery request.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            /// <returns>A Result containing the UserModel.</returns>
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

                    return Result<UserModel>.Success(result, msg);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return Result<UserModel>.Fail(ex.Message);
                }
            }
        }
    }
}
