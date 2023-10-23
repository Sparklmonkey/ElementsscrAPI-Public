using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using ElementscrAPI.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ElementscrAPI.Helpers
{
    public static class ExtendMethods
    {
        public static string GenerateRndSalt()
        {
            // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public static string EncryptPassword(this string password, string salt)
        {
            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            return hashed;
        }

        public static string Sha256(this string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        public static bool isValidKey(this string appKey)
        {
            return false;
        }
        
        
        public static bool IsHostPlayer(this Dictionary<string, PvpRoom> pvpRooms, string playername)
        {
            return pvpRooms.Any(room => room.Value.FirstConnectedPlayer.Username == playername);
        }
    }

    public static class RegexUtilities
    {
        private static readonly string usernameCriteria = @"^[a-zA-Z0-9.,@_-]{3,20}$";
        private static readonly string passwordCriteria = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@!#$%^&*()\-_=+{};:,<.>/?\\[\]|]).{8,30}$";
        
        public static bool UsernameCheck(this string username) => Regex.IsMatch(username, usernameCriteria, RegexOptions.None);
        public static bool PasswordCheck(this string password) => Regex.IsMatch(password, passwordCriteria, RegexOptions.None);
        
        
        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        
        public static string RemoveSquareBrackets(this string response)
        {
            response = response.Replace("[", "");
            response = response.Replace("]", "");
            return response;
        }
    }

}
