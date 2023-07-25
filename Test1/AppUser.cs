namespace Test1;

public class AppUser : IAppUser
{
    public string UserName { get; }
    public Guid UserId { get; }
    public string Email { get; }

    //Should get from JWT Token
    public AppUser(string userName, Guid userId, string email)
    {
        UserName = userName;
        UserId = userId;
        Email = email;
    }
}