using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositories.Interfaces;
using Dapper;
using DAL.Entities;

namespace DAL.Repositories;

public class Repository : BaseRepository, IRepository
{
    public User[] GetAllUsers()
    {
        var connection = GetAndOpenConnection();
        return connection.Query<User>("SELECT * FROM users").ToArray();
    }

    private int GetMaxUserId()
    {
        string query = "SELECT id FROM users";
        var connection = GetAndOpenConnection();
        var result = connection.Query<int>(query).ToArray();
        if (result.Length > 0)
        {
            return result.Max();
        }
        return 0;
    }

    private int GetMaxSessionId()
    {
        string query = "SELECT id FROM sessions";
        var connection = GetAndOpenConnection();
        var result = connection.Query<int>(query).ToArray();
        if (result.Length > 0)
        {
            return result.Max();
        }
        return 0;
    }

    public void RegisterUser(string userName, string email,
        string passwordHash, string role)
    {
        string query = $"INSERT INTO users (id, username, email, " +
            $"password_hash, role, created_at, updated_at) VALUES" +
            $"({GetMaxUserId() + 1}, '{userName}', '{email}', '{passwordHash}'" +
            $", '{role}', @Created, @Created)";
        var sqlParams = new
        {
            Created = DateTime.Now
        };
        var connection = GetAndOpenConnection();
        connection.Execute(new CommandDefinition(query, sqlParams));
    }

    public void UpdateUser(string userName, string email,
        string passwordHash, string role)
    {
        string query = $"UPDATE users SET email='{email}', " +
            $"password_hash='{passwordHash}', role='{role}', updated_at=@Updated " +
            $"WHERE username='{userName}'";
        var sqlParams = new
        {
            Updated = DateTime.Now
        };
        var connection = GetAndOpenConnection();
        connection.Execute(new CommandDefinition(query, sqlParams));
    }

    public void CreateSession(string userName,
        string sessionToken, DateTime expiresAt)
    {
        string query = $"INSERT INTO sessions (id, user_id, " +
            $"session_token, expires_at) VALUES ({GetMaxSessionId() + 1}, " +
            $"{GetUser(userName)?.Id}, '{sessionToken}', @Expires)";
        var sqlQueryParams = new
        {
            Expires = expiresAt
        };
        var connection = GetAndOpenConnection();
        connection.Execute(new CommandDefinition(query, sqlQueryParams));
    }

    public User? GetUser(string userName)
    {
        string query = $"SELECT * FROM users WHERE username='{userName}'";
        var connection = GetAndOpenConnection();
        var result = connection.Query<User>(query).ToArray();
        if (result.Length == 0)
        {
            return null;
        }
        return result[0];
    }

    public bool IsAuthorized(int userId)
    {
        string query = $"SELECT MAX(expires_at) FROM sessions WHERE user_id={userId}";
        var connection = GetAndOpenConnection();
        DateTime?[] result = connection.Query<DateTime?>(query).ToArray();
        if (result is null || result.Length == 0 || result[0] is null)
        {
            return false;
        }
        return DateTime.Now < result[0];
    }

    private int GetMaxDishId()
    {
        string query = "SELECT id FROM dishes";
        var connection = GetAndOpenConnection();
        var result = connection.Query<int>(query).ToArray();
        if (result.Length > 0)
        {
            return result.Max();
        }
        return 0;
    }

    public void CreateDish(string name, string description,
        decimal price, int quantity)
    {
        string query = $"INSERT INTO dishes (id, name, " +
            $"description, price, quantity) " +
            $"VALUES ({GetMaxDishId() + 1}, " +
            $"'{name}', '{description}', {price}, {quantity})";
        var connection = GetAndOpenConnection();
        connection.Execute(query);
    }

    public void UpdateDish(string name, string description,
        decimal price, int quantity)
    {
        string query = $"UPDATE dishes SET description='{description}', " +
            $"price={price}, quantity={quantity} WHERE name='{name}'";
        var connection = GetAndOpenConnection();
        connection.Execute(query);
    }

    public Dish? GetDish(string name)
    {
        string query = $"SELECT * FROM dishes WHERE name='{name}'";
        var connection = GetAndOpenConnection();
        var result = connection.Query<Dish>(query).ToArray();
        if (result.Length == 0)
        {
            return null;
        }
        return result[0];
    }

    public Dish[] GetAllDishes()
    {
        var connection = GetAndOpenConnection();
        return connection.Query<Dish>("SELECT * FROM dishes").ToArray();
    }
}