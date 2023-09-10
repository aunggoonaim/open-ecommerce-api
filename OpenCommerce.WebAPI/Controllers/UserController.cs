using OpenCommerce.Application.Query.User.GetUser;
using OpenCommerce.Application.Query.User.GetUserAll;
using OpenCommerce.Application.Query.User.GetUserLogin;
using OpenCommerce.Domain.DataTransferObject;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OpenCommerce.WebAPI.Controllers;


[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpGet]
    [Route("getById/{id}")]
    public async Task<ActionResult<JsonResponse<GetUserResponse>>> GetById(
        string id,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetUserRequest(id), cancellationToken);
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    [Route("all")]
    public async Task<ActionResult<JsonResponse<GetUserAllResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetUserAllRequest(), cancellationToken);

        if (response.IsError)
        {
            return BadRequest(response.Message);
        }

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<JsonResponse<GetUserLoginResponse>>> Login(
        GetUserLoginRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);

        if (response.IsError)
        {
            return BadRequest(response.Message);
        }

        return Ok(response);
    }
}