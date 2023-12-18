using Haskap.DddBase.Domain;
using System.Security.Cryptography;

namespace Haskap.Recipe.Domain.UserAggregate;
public class Salt : ValueObject
{
    public string Value { get; private set; }

    private Salt()
    {

    }

    public static Salt Generate(int lengthInBits = 128)
    {
        byte[] saltBytes = RandomNumberGenerator.GetBytes(lengthInBits / 8);
        var saltValue = Convert.ToBase64String(saltBytes);

        var salt = new Salt
        {
            Value = saltValue
        };

        return salt;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
