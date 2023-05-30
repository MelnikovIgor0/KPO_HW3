using System.Text.Json;
using WebApplicationAuthorizationMicroService.Extensions;

namespace WebApplicationAuthorizationMicroService.NamingPolicies;

public class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name) =>
            name.ToSnakeCase();
}