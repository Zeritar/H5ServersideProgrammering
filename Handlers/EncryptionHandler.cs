using Microsoft.AspNetCore.DataProtection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace H5ServersideProgrammering.Handlers;

public class SymmetricEncryptionHandler
{
    private readonly IDataProtector _protector;

    public SymmetricEncryptionHandler(IDataProtectionProvider protectionProvider)
    {
        _protector = protectionProvider.CreateProtector("NielsErMinFavoritLærer");
    }

    public string Encrypt(string input)
    {
        return _protector.Protect(input);
    }

    public string Decrypt(string input)
    {
        return _protector.Unprotect(input);
    }
}

public class AsymmetricEncryptionHandler
{
    private string _privateKey;
    private string _publicKey;
    private readonly IConfiguration _configuration;

    public AsymmetricEncryptionHandler(IConfiguration configuration)
    {
        _configuration = configuration;
        string privateKeyPath = _configuration["Keys:PrivateKey"];
        string publicKeyPath = _configuration["Keys:PublicKey"];

        if (!File.Exists(privateKeyPath) || !File.Exists(publicKeyPath))
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                _privateKey = rsa.ToXmlString(true);
                _publicKey = rsa.ToXmlString(false);

                Directory.CreateDirectory(Path.GetDirectoryName(privateKeyPath));
                Directory.CreateDirectory(Path.GetDirectoryName(publicKeyPath));

                File.Create(privateKeyPath).Close();
                File.Create(publicKeyPath).Close();

                StreamWriter streamWriter = new StreamWriter(privateKeyPath);
                streamWriter.Write(_privateKey);
                streamWriter.Close();

                streamWriter = new StreamWriter(publicKeyPath);
                streamWriter.Write(_publicKey);
                streamWriter.Close();
            }
        }
        else
        {
            _privateKey = File.ReadAllText(privateKeyPath);
            _publicKey = File.ReadAllText(publicKeyPath);
        }
        //using (var rsa = new RSACryptoServiceProvider())
        //{
        //    _privateKey = rsa.ToXmlString(true);
        //    _publicKey = rsa.ToXmlString(false);
        //}
        

    }

    public string Encrypt(string input)
    {
        return AsymmetricEncryptor.Encrypt(input, _publicKey);
    }

    public string Decrypt(string input)
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(_privateKey);
            var data = Convert.FromBase64String(input);
            var decryptedData = rsa.Decrypt(data, true);
            return Encoding.UTF8.GetString(decryptedData);
        }
    }
}

public class AsymmetricEncryptor
{
    public static string Encrypt(string input, string publicKey)
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(publicKey);
            var data = Encoding.UTF8.GetBytes(input);
            var encryptedData = rsa.Encrypt(data, true);
            return Convert.ToBase64String(encryptedData);
        }
    }
}
