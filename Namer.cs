using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarisoftCleanDesktop
{   //this object handles naming moved items - especially those that have name conflicts with files in their destination folders.
    class Namer
    {
        String file_name;
        String destination;
        public Namer(String file_name, String destination)
        {
            this.file_name = file_name;
            this.destination = destination;
        }



        public String getName()
        {

            while (new FileInfo(destination+"\\"+file_name).Exists)
            {
                file_name = rename(file_name);
            }


            return file_name;

        }



        //check if name exists in folder
        private Boolean exists_in_folder(String file_name, String destination){

            return new FileInfo(destination + "\\" + file_name).Exists;
        }

        //add 1 to name
        public String rename(String file_name)
        { 
            if (file_name.Length < 1)
            {
                return "-1";
            }
            String extension = new FileInfo(file_name).Extension;
            String name = file_name;
            if (extension != null && extension.Length > 0)
            {
                name = file_name.Replace(extension, "");
            }

            //if there's no digit at end of filename, just add 1
            if (name.Length > 1 && !Char.IsDigit(name[name.Length - 1]))
            {
                name = name + "1" + extension;
                return name;
            }

             
            String digits = "";
            int i = name.Length-1;
            int index = 1;

            //loop through string from back to get all digits 
            while (i > (-1) && Char.IsDigit(name[i]))
            { 
                i--;
            } 


               //capture digits at end of string 
               digits = name.Substring(i+1, 1);
                
                //convert to integer
                index = Convert.ToInt16(digits);

        
            //rename file with new index (You'd have to replace occurence of former index, hence the replace statement)
            name = name.Replace(""+index,"") + (index+1) + extension;

            return name;
        }
         


    }


    public class testNamer{
        Namer namer;

        public testNamer()
        {
            namer = new Namer(null, null);
        }

        public void test_renamer(){
            String name = "New Folder.txt";
            for(int i = 0; i < 10; i++){

                name = namer.rename(name);
                log(name);
            }

        }



        private void log(String message){
            Log.getInstance().i(message);
        }

    }
}
