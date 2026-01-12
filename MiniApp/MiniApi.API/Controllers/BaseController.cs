using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiniApp.DTOs.Users;
using MiniApp.BLL.Features.Queries.Users.GetById;

namespace MiniApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
   
}
