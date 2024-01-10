using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Abstractions
{
    [ApiController]
    public abstract class BaseController<T> : ControllerBase
    {
        private IMediator _mediatorInstance;
        private IMapper _mapperInstance;
        protected IMediator _mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();
        protected IMapper _mapper => _mapperInstance ??= HttpContext.RequestServices.GetService<IMapper>();

    }
}
