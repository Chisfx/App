using App.Application.Features.Queries;
using App.Domain.DTOs;
using AspNetCoreHero.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
namespace App.Application.Features.Utils
{
    public class GetGroupHostQuery : IRequest<Result<List<GroupHostModel>>>
    {
        public class GetGroupHostQueryHandler : IRequestHandler<GetGroupHostQuery, Result<List<GroupHostModel>>>
        {
            private readonly IMediator _mediator;
            private readonly ILogger<GetGroupHostQuery> _logger;
            public GetGroupHostQueryHandler(IMediator mediator, ILogger<GetGroupHostQuery> logger)
            {
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<Result<List<GroupHostModel>>> Handle(GetGroupHostQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    List<GroupHostModel> entities;
                    var response = await _mediator.Send(new GetAllUserQuery());

                    if (response.Succeeded)
                    {
                        var random = new Random();
                        entities = response.Data.GroupBy(x => new MailAddress(x.Email).Host).Select(x => new GroupHostModel
                        {
                            Host = x.Key,
                            Count = x.Count(),
                            Color = String.Format("#{0:X6}", random.Next(0x1000000))
                        }).ToList();
                    }
                    else
                    {
                        throw new Exception(response.Message);
                    }

                    return await Result<List<GroupHostModel>>.SuccessAsync(entities);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return await Result<List<GroupHostModel>>.FailAsync(ex.Message);
                }
            }
        }

    }
}
