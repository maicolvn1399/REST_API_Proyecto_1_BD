using System.Security.Cryptography;
using System.Text;

namespace REST_API_GymTEC.Database_Resources
{

    public class Encryption
    {

        Encryption()
        {

        }

        public static string encrypt_password(string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] array = Encoding.UTF8.GetBytes(password);
            array = md5.ComputeHash(array);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in array)
            {
                stringBuilder.Append(b.ToString("x2").ToLower());
            }

            return stringBuilder.ToString();
        }

        

    }
}
