using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace e_com.Utils
{
    public class UniqueImageFileNameGenerator
    {
        public static string GenerateUniqueImageFileName(string baseName, string fileExtension)
        {
            string uniqueIdentifier = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string uniqueFileName = $"{baseName}_{uniqueIdentifier}{fileExtension}";
            return uniqueFileName;
        }
    }
}