using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Haskap.Recipe.Domain.RecipeAggregate;
public class Slug : ValueObject
{
    public string Value { get; private set; }


    private Slug()
    {
        
    }

    //public Slug(string value)
    //{
    //    Guard.Against.NullOrWhiteSpace(value);

    //    Value = value;
    //}

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value; 
    }

    public static Slug Generate(string text, int substringLength)
    {
        Guard.Against.NullOrWhiteSpace(text);
        Guard.Against.NegativeOrZero(substringLength);

        var value = RemoveAccent(text).ToLower();

        // invalid chars           
        value = Regex.Replace(value, @"[^a-z0-9\s-]", "");

        // convert multiple spaces into one space   
        value = Regex.Replace(value, @"\s+", " ").Trim();

        value = value.Substring(0, value.Length >= substringLength ? substringLength : value.Length).Trim();

        // convert spaces into hyphens
        value = Regex.Replace(value, @"\s", "-");

        value = value + "_" + Guid.NewGuid().ToString("N");

        return new Slug { Value = value };
    }

    private static string RemoveAccent(string text)
    {
        //byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
        //return System.Text.Encoding.ASCII.GetString(bytes);

        text = text.Normalize(NormalizationForm.FormD);
        char[] chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();

        return new string(chars).Normalize(NormalizationForm.FormC);
    }
}
