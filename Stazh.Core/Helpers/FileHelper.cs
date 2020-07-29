
using System;
using System.Linq;

namespace Stazh.Core.Helpers
{
    public class FileHelper
    {
        public static string ObscureFileName(string fileName)
        {
            //Could have an option to use a FileInfo wrapper if the file exists on disk.
            //For now its just a way to create a new obscured GUID filename.

            var fileExtension = fileName.Split(".").Last();
            var newFileName = Guid.NewGuid().ToString() + "." + fileExtension;
            return newFileName;
        }
    }
}