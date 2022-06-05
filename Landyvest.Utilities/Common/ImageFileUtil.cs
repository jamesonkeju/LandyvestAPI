using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Utilities.Common
{
   public class ImageFileUtil
    {
        public static bool IsImageExtension(string extension)
        {
            var allowedExtensions = new List<string> { "jpeg", "jpg", "png" };
            foreach (var item in allowedExtensions)
            {
                if (item.ToLower() == extension.ToLower())
                    return true;
            }
            return false;
        }

        public static bool IsVideoExtension(string extension)
        {
            var allowedExtensions = new List<string> { "mp4", "webm", "ogg" };
            foreach (var item in allowedExtensions)
            {
                if (item.ToLower() == extension.ToLower())
                    return true;
            }
            return false;
        }


    }
}
