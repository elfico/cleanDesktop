using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarisoftCleanDesktop
{
    class SpecialFile
    {

        String path;
        public SpecialFile(String path)
        {
            this.path = path; 

        }



            //identify where the belongings of this file are located 
        public String getBelongingsFolder()
        {
            if (path == null || !(new FileInfo(path).Exists))
            {
                return null;
            }


            //read file into memory
            String content = File.ReadAllText(path);  

          
            //these folders are usually started with ./ (atleast for chrome saved web files)
            int i = content.IndexOf("./");

            //this file has no reference to a folder
            if (i == -1)
            {
                return null;
            }

            //advance past the './'
            i += 2;
            content = content.Substring(i);

            //get the termination of this folder name
            int j = content.IndexOf("files");  

            //return a substring of the start of './' and the end which is usually 'files' and add the files back... (winks)
            return content.Substring(0, j)+"files"; 

        }
    }
}
