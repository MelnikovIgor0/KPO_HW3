using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Repositories.Interfaces;

/// <summary>
/// abstract class for repository for operations with db.
/// </summary>
public interface IRepository
{
    /// <summary>
    /// method returns all users.
    /// </summary>
    /// <returns></returns>
    User[] GetAllUsers();

    /// <summary>
    /// method registers new user.
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="email"></param>
    /// <param name="passwordHash"></param>
    /// <param name="role"></param>
    void RegisterUser(string userName, string email,
        string passwordHash, string role);

    /// <summary>
    /// method updates data about user.
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="email"></param>
    /// <param name="passwordHash"></param>
    /// <param name="role"></param>
    void UpdateUser(string userName, string email,
        string passwordHash, string role);

    /// <summary>
    /// methods gets user by id.
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    User GetUser(string userName);

    /// <summary>
    /// method creates new session.
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="sessionToken"></param>
    /// <param name="expiresAt"></param>
    void CreateSession(string userName,
        string sessionToken, DateTime expiresAt);

    /// <summary>
    /// merthod checks is user authorized.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    bool IsAuthorized(int userId);

    /// <summary>
    /// method creates new dish.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="price"></param>
    /// <param name="quantity"></param>
    void CreateDish(string name, string description, 
        decimal price, int quantity);

    /// <summary>
    /// method updates dish.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="price"></param>
    /// <param name="quantity"></param>
    void UpdateDish(string name, string description,
        decimal price, int quantity);

    /// <summary>
    /// method gets dish by name.
    /// </summary>
    /// <param name="Name"></param>
    /// <returns></returns>
    Dish GetDish(string Name);

    /// <summary>
    /// method gets all dishes.
    /// </summary>
    /// <returns></returns>
    Dish[] GetAllDishes();
}
