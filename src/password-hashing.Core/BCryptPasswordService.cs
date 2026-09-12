namespace HashingPlayground.Core;

public class BCryptPasswordService : IPasswordService
{
    public PasswordServiceType Type => PasswordServiceType.BCrypt;

    public string Hash(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);

    public bool Verify(string passwordHash, string password)
        => BCrypt.Net.BCrypt.Verify(passwordHash, password);

    public bool CanHandle(string? passwordHash)
    {
        if(string.IsNullOrWhiteSpace(passwordHash))
            return false;

        try
        {
            BCrypt.Net.BCrypt.InterrogateHash(passwordHash);
            return true;
        }
        catch (BCrypt.Net.HashInformationException)
        {
            return false;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }
}
