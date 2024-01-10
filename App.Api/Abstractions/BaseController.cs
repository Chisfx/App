using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Abstractions
{
    /// <summary>
    /// Base controller for API endpoints.
    /// </summary>
    /// <typeparam name="T">The type of the controller.</typeparam>
    [ApiController]
    public abstract class BaseController<T> : ControllerBase
    {
        private IMediator _mediatorInstance;
        private IMapper _mapperInstance;

        /// <summary>
        /// The mediator instance.
        /// </summary>
        protected IMediator _mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();

        /// <summary>
        /// The mapper instance.
        /// </summary>
        protected IMapper _mapper => _mapperInstance ??= HttpContext.RequestServices.GetService<IMapper>();
    }
}
