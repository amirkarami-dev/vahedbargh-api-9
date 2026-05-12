using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Common.Utility
{
    public class UtilityHelper
    {
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString().ToLower();
            }
        }

       public const string StrNationalCardFront =
            "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";

       public const string StrSanad = "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";
    }
}
