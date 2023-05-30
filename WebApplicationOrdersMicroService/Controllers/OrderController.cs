using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Npgsql;
using Npgsql.NameTranslation;
using Dapper;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using WebApplicationOrdersMicroService.Requests;
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

namespace WebApplicationOrdersMicroService.Controllers;

/// <summary>
/// class of order controller.
/// </summary>
[Route("order")]
[ApiController]
public sealed class OrderController : Controller
{
    private IRepository _repository;

    public OrderController()
    {
        _repository = new Repository();
    }

    /// <summary>
    /// method gets dish by its name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [HttpGet("dish/{name}")]
    public IActionResult GetDish(string name)
    {
        Dish? dish = _repository.GetDish(name);
        if (dish is null)
        {
            return BadRequest("Wtong dish name specified");
        }
        return Ok(dish);
    }

    /// <summary>
    /// method creates new dish.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("create-dish")]
    public IActionResult CreateDish([FromBody] CreateOrUpdateDishRequest request)
    {
        if (_repository.GetDish(request.Name) is not null)
        {
            return BadRequest("Dish with specified name already exists");
        }
        _repository.CreateDish(request.Name, request.Description, request.Price, request.Quantity);
        return Ok("dish created");
    }

    /// <summary>
    /// method updates dish.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("update-dish")]
    public IActionResult UpdateDish([FromBody] CreateOrUpdateDishRequest request)
    {
        if (_repository.GetUser(request.SenderName) is null || _repository.GetUser(request.SenderName).Role != "manager")
        {
            return BadRequest("access denied");
        }
        if (_repository.GetDish(request.Name) is null)
        {
            return BadRequest("there's no dish with specified name");
        }
        _repository.UpdateDish(request.Name, request.Description,
            request.Price, request.Quantity);
        return Ok("dish updated");
    }

    /// <summary>
    /// method returns all dishes.
    /// </summary>
    /// <returns></returns>
    [HttpGet("all-dishes")]
    public IActionResult GetAllDishes()
    {
        return Ok(_repository.GetAllDishes());
    }
}
