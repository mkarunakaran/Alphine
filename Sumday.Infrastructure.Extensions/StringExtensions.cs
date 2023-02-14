using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sumday.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

        public static string Base64Encode(this string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return null;
            }

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            if (string.IsNullOrEmpty(base64EncodedData))
            {
                return null;
            }

            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static byte[] CreateRandomBytes(int length)
        {
            var bytes = new byte[length];
            Rng.GetBytes(bytes);

            return bytes;
        }

        public static string CreateRandomKey(int length)
        {
            var bytes = new byte[length];
            Rng.GetBytes(bytes);

            return Convert.ToBase64String(CreateRandomBytes(length));
        }

        public static string CreateUniqueKey(int length = 8)
        {
            return CreateRandomBytes(length).ToHexString();
        }

        public static string CreateSeriesNumber(string prefix = "MSK")
        {
            return $"{prefix}{DateTime.Now:yyyyMMddHHmmss}{CreateUniqueKey()}";
        }

        public static string ToCamelCase(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            var split = str.Split('.').Select(i => string.Concat(i[0].ToString().ToLower(), i[1..]));

            return string.Join(".", split);
        }

        public static string ToTitleCase(this string str)
        {
            var tokens = str.Split(new[] { " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < tokens.Length; i++)
            {
                var token = tokens[i];
                tokens[i] = token == token.ToUpper()
                    ? token
                    : token.Substring(0, 1).ToUpper() + token[1..].ToLower();
            }

            return string.Join(" ", tokens);
        }

        public static string ToApplicationRoot(this string exePath, bool parent = true)
        {
            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;
            if (string.IsNullOrEmpty(appRoot))
            {
                appRoot = exePath;
            }

            return parent ? Directory.GetParent(appRoot).FullName : appRoot;
        }

        public static string PascalCaseToSentence(this string input)
        {
            if (input == null)
            {
                return string.Empty;
            }

            string output = Regex.Replace(input, @"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])", m => " " + m.Value);
            return output;
        }

        public static string ToPascalCase(this string original)
        {
            return string.Join(".", original.Split(new char[] { ' ', '.' })
                 .Select(w => w.Trim())
                 .Where(w => w.Length > 0)
                 .Select(w => w.Substring(0, 1).ToUpper() + w[1..]));
        }

        public static string AAn(this string source)
        {
            if (source == null)
            {
                return null;
            }

            var firstChar = source.Trim().Substring(0, 1).ToLower();

            return "aeiou".IndexOf(firstChar, StringComparison.CurrentCultureIgnoreCase) > -1
                ? $"an {source}"
                : $"a {source}";
        }

        public static string ToSha256HashString(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            using var sha = SHA256.Create();
            byte[] textData = Encoding.Unicode.GetBytes(text);
            byte[] hash = sha.ComputeHash(textData);
            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }

        public static Type GetTypeByName(this string className)
        {
            Type returnVal = default;

            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] assemblyTypes = a.GetTypes();
                for (int j = 0; j < assemblyTypes.Length; j++)
                {
                    if (assemblyTypes[j].Name == className)
                    {
                        returnVal = assemblyTypes[j];
                    }
                }
            }

            return returnVal;
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public static string TrimEnd(this string input, string suffixToRemove, StringComparison comparisonType = StringComparison.CurrentCulture)
        {
            if (suffixToRemove != null && input.EndsWith(suffixToRemove, comparisonType))
            {
                return input.Substring(0, input.Length - suffixToRemove.Length);
            }

            return input;
        }

        public static string TrimStart(this string source, string trim, StringComparison stringComparison = StringComparison.Ordinal)
        {
            if (source == null)
            {
                return null;
            }

            var s = source;
            while (s.StartsWith(trim, stringComparison))
            {
                s = s[trim.Length..];
            }

            return s;
        }

        public static string TrimAfter(this string source, string trim)
        {
            var index = source.IndexOf(trim, StringComparison.Ordinal);
            if (index > 0)
            {
                source = source.Substring(0, index);
            }

            return source;
        }

        public static bool TryParse(this string strInput, out JObject output)
        {
            if (string.IsNullOrWhiteSpace(strInput))
            {
                output = null;
                return false;
            }

            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) ||
                (strInput.StartsWith("[") && strInput.EndsWith("]")))
            {
                try
                {
                    output = JObject.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException)
                {
                    output = null;
                    return false;
                }
                catch (Exception)
                {
                    output = null;
                    return false;
                }
            }
            else
            {
                output = null;
                return false;
            }
        }

        public static Guid? FromBase64(this string encoded)
        {
            try
            {
                encoded = encoded.Replace("-", "/");
                encoded = encoded.Replace("_", "+");
                var buffer = Convert.FromBase64String($"{encoded}==");
                return new Guid(buffer);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
