using System.Text.Json;
using WebApplicationOrdersMicroService.Extensions;

namespace WebApplicationOrdersMicroService.NamingPolicies;

public class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name) =>
            name.ToSnakeCase();
}