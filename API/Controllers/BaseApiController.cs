using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // hence our route is /api/activities or api/weatherforecase since thats the controller name
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediator;
        // if _mediator is null, assign the HttpContext... to Mediator; else asign _mediator
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        
    }
}