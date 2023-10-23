using System.Net;
using ElementscrAPI.Data;
using ElementscrAPI.Entities;
using ElementscrAPI.Identity;
using ElementscrAPI.Models;
using ElementscrAPI.Models.Requests;
using ElementscrAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace ElementscrAPI.Controllers;

[Route("admin")]
public class AdminController : Controller
{
    private readonly IAdminRepository _repository;
    
    public AdminController(IAdminRepository repository)
    {
        _repository = repository;
    }
    
    [Route("register-admin", Name = "register-admin")]
    [HttpPut]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<string>> RegisterAdmin([FromBody] LoginRequest loginRequest)
    {
        return Ok(await _repository.RegisterAdmin(loginRequest));
    }
    
    [Route("test-migrate", Name = "test-migrate")]
    [HttpGet]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<int>> TestMigrate()
    {
        return Ok(await _repository.TestMigrate());
    }
    
    [Route("login-admin", Name = "login-admin")]
    [HttpPost]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<string>> LoginAdmin([FromBody] LoginRequest loginRequest)
    {
        return Ok(await _repository.LoginAdmin(loginRequest));
    }
    
    [Route("add-new-code", Name = "add-new-code")]
    [Authorize(Policy = IdentityHolder.AdminUserPolicyName)]
    [HttpPut]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> AddNewCode([FromBody] CodeDetails codeDetails)
    {
        var bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        return Ok(await _repository.AddNewCode(codeDetails, bearerToken));
    }
    
    [Route("reset-player-password", Name = "reset-player-password")]
    [Authorize(Policy = IdentityHolder.AdminUserPolicyName)]
    [HttpPost]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> ResetPlayerPassword([FromBody] PlayerPasswordResetRequest passwordResetRequest)
    {
        var bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        return Ok(await _repository.ResetPlayerPassword(passwordResetRequest, bearerToken));
    }
}