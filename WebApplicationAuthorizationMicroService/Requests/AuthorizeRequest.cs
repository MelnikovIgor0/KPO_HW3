namespace WebApplicationAuthorizationMicroService.Requests;

public sealed record AuthorizeRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
}
