using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Utilities.Common
{
    public class RemoveCharUtil
    {
        public static string RemoveInvalidCharacters(string unique)
        {
            //" \ / : | < > * ?
            unique = unique
                .Replace(" ", "")
                .Replace(",", "")
                .Replace(@"\", "")
                .Replace(@"/", "")
                .Replace(":", "")
                .Replace("|", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("*", "")
                .Replace("?", "")
                .Replace("\"", "");

            return unique;
        }

    }
}
