using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace LarisoftCleanDesktop
{
    //app settings generally
    class Preferences
    {
        public static String ExtensionListFileName = "extensions.txt";
        public static String Desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static String LogStorage = getStorage() + "\\log.txt";
        public static String ExtensionListStorage = getStorage() + "\\extensions.txt";
        public static String getStorage()
        {

          String  storage = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\CleanDesktop";
          if (!new DirectoryInfo(storage).Exists)
          {
              Directory.CreateDirectory(storage);
          }

          return storage;

        }

        public static Boolean isFirstTimeRunning()
        {
            return (!new FileInfo(getStorage()+"\\"+ExtensionListFileName).Exists);
        }

          
    }
}
