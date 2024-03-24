using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OLAP.API.Application.Commands.Account;
using OLAP.API.Models.Identity;
using OLAP.API.Services;

namespace OLAP.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{api-version:apiVersion}/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IAccountService _accountService;

        public AccountController(IMediator mediator, SignInManager<ApplicationUser> signInManager, IAccountService accountService)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<object>> Login([FromBody] LoginCommand model, CancellationToken cancellationToken = default)
        {
            //var result = await _mediator.Send(command, cancellationToken);
            //if (!result)
            //    return BadRequest("Incorrect credentials.");

            //return Ok(result);

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                var returnUrl = $"/api/v1.0/Account/Management?";// model.ReturnUrl;

                //ViewData["ReturnUrl"] = returnUrl;
                if (ModelState.IsValid)
                {
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    var result = await _accountService.LoginByEMail(model, cancellationToken);
                    //var result = await _mediator.Send(model, cancellationToken);
                    if (result.SignInResult.Succeeded)
                    {
                        var responseJson = JsonConvert.SerializeObject(result.TokenResponse);
                        Response.Headers.Add("access_token", responseJson);

                        Url.ActionContext.RouteData.DataTokens.Add("access_token", result.TokenResponse);
                        //_logger.LogInformation(1, "User logged in.");
                        return new OkObjectResult(new { Token = result.TokenResponse.Token });
                    }
                    else
                    {
                        return BadRequest("Invalid username and password");
                    }
                }

                throw new NotImplementedException("BuildLogin not implemented");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<bool>> RegisterUser([FromBody] RegisterUserCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            if (!result)
                return BadRequest("User has not been created.");

            return Ok(result);
        }
    }
}
