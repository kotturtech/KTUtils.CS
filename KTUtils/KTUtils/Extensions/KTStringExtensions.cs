using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotturTech.Extensions
{
    /// <summary>
    /// Contains extension methods for strings
    /// </summary>
    public static class KTStringExtensions
    {
        /// <summary>
        /// Calculates the length of common prefix of two strings
        /// </summary>
        /// <param name="str">String</param>
        /// <param name="prefix">Prefix</param>
        /// <param name="caseSensitive">Indicates if the calculation will consider the character case of string and prefix.
        /// Default value is false</param>
        /// <returns>Length of common prefix of the strings</returns>
        public static int CommonPrefixLength(this string str, string prefix, bool caseSensitive = false)
        {
            int limit = Math.Min(str.Length, prefix.Length);

            if(!caseSensitive)
            {
                str = str.ToLower();
                prefix = prefix.ToLower();
            }

            int prefixLength = 0;
            for(int i = 0; i < limit; i++)
            {
                if(str[i] == prefix[i])
                    prefixLength++;
                else
                    break;
            }

            return prefixLength;
        }
    }
}
