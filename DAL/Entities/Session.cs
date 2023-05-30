namespace DAL.Entities;

public sealed record Session
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string SessionToken { get; set; }
    public DateTime ExpiresAt { get; set; }
}
