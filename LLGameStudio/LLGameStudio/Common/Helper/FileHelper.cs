using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLGameStudio.Common.Helper
{
    class FileHelper
    {
        public static void CopyDirectory(string sourceDirectory,string destination,bool overWrite = true)
        {
            var sourceFilesPath = Directory.GetFileSystemEntries(sourceDirectory);
 
             for (int i = 0; i < sourceFilesPath.Length; i++)
             {
                 var sourceFilePath = sourceFilesPath[i];
                 var directoryName = Path.GetDirectoryName(sourceFilePath);
                 var forlders = directoryName.Split('\\');
                 var lastDirectory = forlders[forlders.Length - 1];
                 var dest = Path.Combine(destination, lastDirectory);
 
                 if (File.Exists(sourceFilePath))
                 {
                     var sourceFileName = Path.GetFileName(sourceFilePath);
                     if (!Directory.Exists(dest))
                     {
                         Directory.CreateDirectory(dest);
                     }
                     File.Copy(sourceFilePath, Path.Combine(dest, sourceFileName), overWrite);
                 }
                 else
                 {
                    CopyDirectory(sourceFilePath, dest, overWrite);
                 }
             }
        }
    }
}
