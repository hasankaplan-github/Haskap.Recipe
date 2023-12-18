using Ardalis.GuardClauses;
using Haskap.Recipe.Domain.Shared.Consts;
using Haskap.DddBase.Domain;
using System.Security.Cryptography;
using System.Text;

namespace Haskap.Recipe.Domain.UserAggregate;
public class Password : ValueObject
{
    public string HashedValue { get; private set; }
    public string ClearValue { get; private set; }
    public Salt Salt { get; private set; }

    private Password()
    {
    }

    public Password(string clearPassword, Salt salt)
    {
        Guard.Against.NullOrWhiteSpace(clearPassword, nameof(clearPassword), "Şifre boş olamaz!");
        Guard.Against.InvalidInput(clearPassword, nameof(clearPassword), x => x.Length >= PasswordConsts.MinPasswordLength, "Şifre en az altı karakter uzunluğunda olmalıdır!");
        Guard.Against.InvalidInput(clearPassword, nameof(clearPassword), x => x.Any(pc => !char.IsLetterOrDigit(pc)), "Şifre en az bir adet özel karakter içermelidir!");
        Guard.Against.InvalidInput(clearPassword, nameof(clearPassword), x => x.Any(char.IsDigit), "Şifre en az bir adet rakam içermelidir!");
        Guard.Against.InvalidInput(clearPassword, nameof(clearPassword), x => x.Any(char.IsUpper), "Şifre en az bir adet büyük harf içermelidir!");
        Guard.Against.InvalidInput(clearPassword, nameof(clearPassword), x => x.Any(char.IsLower), "Şifre en az bir adet küçük harf içermelidir!");

        Guard.Against.Null(salt, nameof(salt), "Salt parametresi null olamaz!");

        ClearValue = clearPassword;
        Salt = salt;
        HashedValue = ComputeHash(clearPassword, salt);
    }

    public static string ComputeHash(string clearPassword, Salt salt)
    {
        var clearPasswordAndSaltBytes = Encoding.UTF8.GetBytes(clearPassword + salt.Value);
        using var hashAlgorithm = SHA256.Create(); 
        var hashedPasswordBytes = hashAlgorithm.ComputeHash(clearPasswordAndSaltBytes);
        var hashedPassword = Convert.ToBase64String(hashedPasswordBytes);

        return hashedPassword;
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return HashedValue;
        yield return Salt;
    }
}
