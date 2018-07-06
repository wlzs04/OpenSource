using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLGameStudio.Common.Helper
{
    /// <summary>
    /// 与操作文件相关的帮助类
    /// </summary>
    class FileHelper
    {
        /// <summary>
        /// 拷贝指定目录下的文件到新目录中
        /// </summary>
        /// <param name="sourceDirectory">源目录</param>
        /// <param name="destination">新目录</param>
        /// <param name="overWrite">新目录存在时是否复写，默认为真</param>
        public static void CopyDirectory(string sourceDirectory,string destination,bool overWrite = true)
        {
            var sourceFilesPath = Directory.GetFileSystemEntries(sourceDirectory);
 
             for (int i = 0; i < sourceFilesPath.Length; i++)
             {
                 string sourceFilePath = sourceFilesPath[i];
                 string directoryName = Path.GetDirectoryName(sourceFilePath);
                 var forlders = directoryName.Split('\\');
                 string lastDirectory = forlders[forlders.Length - 1];
                 string dest = Path.Combine(destination, lastDirectory);
 
                 if (File.Exists(sourceFilePath))
                 {
                     string sourceFileName = Path.GetFileName(sourceFilePath);
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
