using System.Security.Cryptography;
using System.Text;

namespace REST_API_GymTEC.Database_Resources
{
    /// <summary>
    /// Class to encrypt passwords to be saved to the database
    /// </summary>
    public class Encryption
    {

        Encryption()
        {

        }

        /// <summary>
        /// Method to encrypt passwords 
        /// </summary>
        /// <param name="password">  receives a string called password to be encrypted </param>
        /// <returns> returns a string that corresponds to the encrypted password to be saved into the database </returns>
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
