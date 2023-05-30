using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Npgsql;
using Npgsql.NameTranslation;
using Dapper;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using WebApplicationAuthorizationMicroService.Requests;
using System.Text.RegularExpressions;
using DAL.Entities;
using System.Security.Cryptography;
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace WebApplicationAuthorizationMicroService.Controllers;

/// <summary>
/// class of authorization controller.
/// </summary>
[ApiController]
[Route("user")]
public sealed class UserController : Controller
{
    private IRepository _repository;

    private bool IsValidEmail(string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith("."))
        {
            return false; // suggested by @TK-421
        }
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }

    private string ComputeHash(string password)
    {
        byte[] hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.Unicode.GetBytes(password));
        StringBuilder sb = new StringBuilder();
        foreach (byte element in hash)
        {
            sb.Append(element.ToString());
        }
        return sb.ToString();
    }

    private string CreateJwtToken(User user)
    {
        var signinKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));
        var signInCredentials = new SigningCredentials(signinKey, 
            SecurityAlgorithms.HmacSha256);
        var claims = new Claim[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username)
            };
        var jwt = new JwtSecurityToken(claims: claims, 
            signingCredentials: signInCredentials);
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public UserController()
    {
        _repository = new Repository();
    }

    /// <summary>
    /// method creates user.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("create-user")]
    public IActionResult CreateUser([FromBody] CreateOrUpdateUserRequest request)
    {
        if (_repository.GetUser(request.UserName) is not null)
        {
            return BadRequest("User with specified username exists");
        }
        if (!IsValidEmail(request.Email))
        {
            return BadRequest("Specified email is not valid");
        }
        if (request.Role != "customer" && request.Role != "chef" && request.Role != "manager")
        {
            return BadRequest("Specified role is not valid");
        }
        if (request.Password.Length < 6)
        {
            return BadRequest("Specified password too short");
        }
        _repository.RegisterUser(request.UserName, request.Email,
            ComputeHash(request.Password), request.Role);
        return Ok("User successfully registred");
    }

    /// <summary>
    /// method gets user.
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    [HttpGet("user/{userName}")]
    public IActionResult GetUser(string userName)
    {
        var user = _repository.GetUser(userName);
        if (user is null)
        {
            return BadRequest("There's no user with specified username");
        }
        return Ok(user);
    }

    /// <summary>
    /// method authorizes user.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("authorize")]
    public IActionResult Authorize([FromBody] AuthorizeRequest request)
    {
        User? user = _repository.GetUser(request.UserName);
        if (user is null)
        {
            return BadRequest("Wrong username specified");
        }
        if (user.PasswordHash != ComputeHash(request.Password))
        {
            return BadRequest("Wrong password specified");
        }

        string jwtText = CreateJwtToken(user);
        _repository.CreateSession(request.UserName, jwtText, DateTime.Now + new TimeSpan(0, 15, 0));
        return Ok($"user {request.UserName} authorized");
    }

    private bool IsUserAuthorized(string userName)
    {
        User? user = _repository.GetUser(userName);
        if (user is null)
        {
            throw new ArgumentException("Unknown User");
        }
        return _repository.IsAuthorized(user.Id);
    }


    /// <summary>
    /// method checks if user authorized.
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    [HttpGet("is-authorized/{userName}")]
    public IActionResult IsAuthorized(string userName)
    {
        try
        {
            return Ok(IsUserAuthorized(userName));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


    /// <summary>
    /// method updates users data.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("update")]
    public IActionResult UpdateUser([FromBody] CreateOrUpdateUserRequest request)
    {
        if (!IsUserAuthorized(request.UserName))
        {
            return BadRequest($"user {request.UserName} is not authorized");
        }
        User? user = _repository.GetUser(request.UserName);
        if (user is null)
        {
            return BadRequest("Wrong username specified");
        }
        if (!IsValidEmail(request.Email))
        {
            return BadRequest("Specified email is not valid");
        }
        if (request.Role != "customer" && request.Role != "chef" && request.Role != "manager")
        {
            return BadRequest("Specified role is not valid");
        }
        if (request.Password.Length < 6)
        {
            return BadRequest("Specified password too short");
        }
        _repository.UpdateUser(request.UserName, request.Email, 
            ComputeHash(request.Password), request.Role);
        return Ok("user's data updated");
    }
}
