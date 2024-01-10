using App.Application.Features.Queries;
using App.Domain.DTOs;
using AspNetCoreHero.Results;
using MediatR;
using Microsoft.Extensions.Logging;
namespace App.Application.Features.Utils
{
    public class GetGroupAgeQuery : IRequest<Result<List<GroupAgeModel>>>
    {
        public class GetGroupAgeQueryHandler : IRequestHandler<GetGroupAgeQuery, Result<List<GroupAgeModel>>>
        {
            private readonly IMediator _mediator;
            private readonly ILogger<GetGroupAgeQuery> _logger;
            public GetGroupAgeQueryHandler(IMediator mediator, ILogger<GetGroupAgeQuery> logger)
            {
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<Result<List<GroupAgeModel>>> Handle(GetGroupAgeQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    List<GroupAgeModel> entities;
                    var response = await _mediator.Send(new GetAllUserQuery());

                    if (response.Succeeded)
                    {
                        var random = new Random();
                        entities = response.Data.GroupBy(x => x.Age).Select(x => new GroupAgeModel
                        {
                            Age = x.Key,
                            Count = x.Count(),
                            Color = String.Format("#{0:X6}", random.Next(0x1000000))
                        }).ToList();
                    }
                    else
                    {
                        throw new Exception(response.Message);
                    }

                    return await Result<List<GroupAgeModel>>.SuccessAsync(entities);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return await Result<List<GroupAgeModel>>.FailAsync(ex.Message);
                }
            }
        }

    }
}
