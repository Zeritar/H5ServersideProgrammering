using System.Security.Cryptography;
using System.Text;

namespace H5ServersideProgrammering.Handlers
{
    public class HashingHandler
    {
        // Hash using MD5
        public byte[] HashMD5(string input)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);
                return hashBytes;
            }
        }

        // Hash using SHA256
        public byte[] HashSHA256(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(input);
                var hashBytes = sha256.ComputeHash(inputBytes);
                return hashBytes;
            }
        }

        //Hash using HMAC
        public byte[] HashHMAC(string input, byte[] key)
        {
            using (var hmac = new HMACSHA256(key))
            {
                var inputBytes = Encoding.UTF8.GetBytes(input);
                var hashBytes = hmac.ComputeHash(inputBytes);
                return hashBytes;
            }
        }

        //Hash using PBKDF2
        public byte[] HashPBKDF2(string input, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(input, salt, 100, HashAlgorithmName.SHA384))
            {
                return pbkdf2.GetBytes(20);
            }
        }

        //Hash using BCrypt
        public byte[] HashBCrypt(string input, string salt)
        {
            return Encoding.UTF8.GetBytes(BCrypt.Net.BCrypt.HashPassword(input, salt));
        }

        //Hash and return as string, byte array, utf, hex or bit string based on enums, default byte array
        public dynamic Hash(string input, HashAlgorithm algorithm)
        {
            return Hash(input, algorithm, HashOutput.ByteArray);
        }

        //Hash and return as string, byte array, utf, hex or bit string based on enums
        public dynamic Hash(string input, HashAlgorithm algorithm, HashOutput output)
        {
            byte[] result;
            switch(algorithm)
            {
                case HashAlgorithm.MD5:
                    result = HashMD5(input);
                    break;
                case HashAlgorithm.SHA256:
                    result = HashSHA256(input);
                    break;
                case HashAlgorithm.HMAC:
                    result = HashHMAC(input, Encoding.UTF8.GetBytes("key"));
                    break;
                case HashAlgorithm.PBKDF2:
                    result = HashPBKDF2(input, Encoding.UTF8.GetBytes("salt"));
                    break;
                case HashAlgorithm.BCrypt:
                    result = HashBCrypt(input, "salt");
                    break;
                default:
                    result = null;
                    break;
            }
            if (result == null) return "NULL";

            switch(output)
            {
                case HashOutput.String:
                    return Encoding.ASCII.GetString(result);
                case HashOutput.ByteArray:
                    return result;
                case HashOutput.UTF8:
                    return Encoding.UTF8.GetString(result);
                case HashOutput.Hex:
                    return BitConverter.ToString(result).Replace("-","");
                case HashOutput.BitString:
                    return BitConverter.ToString(result);
                default:
                    return "NULL";
            }
        }

        public static string ByteArrayToString(byte[] ba)
        {
            return "[" + BitConverter.ToString(ba).Replace("-", ", ").ToUpper() + "]";
        }
    }

    public enum HashOutput
    {
        String,
        ByteArray,
        UTF8,
        Hex,
        BitString
    }
    public enum HashAlgorithm
    {
        MD5,
        SHA256,
        HMAC,
        PBKDF2,
        BCrypt
    }
}
