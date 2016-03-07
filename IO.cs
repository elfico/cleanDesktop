using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace LarisoftCleanDesktop
{   

    //singleton class for all input/output operations
    //saves objects in json format
    class IO
    {
        private static IO instance;
        List<IO_Listener> Observers =  new List<IO_Listener>();
           
        //hide instantiation
        private IO() { } 
       
        public static IO getInstance(){ 

            if (instance == null)
            {
                instance = new IO(); 
            } 
            return instance; 
        }


        public void addObserver(IO_Listener listener)
        {
            this.Observers.Add(listener);
        }

        //save any type of object
        public void saveObject(object obj, String location)
        { 
            JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            String seria = serializer.Serialize(obj);
            save(location, seria);

            foreach (IO_Listener i in Observers)
            {
                i.objectsChanged(obj);
            }

        }


        // write string to file
        public void save(String path, String content)
        {
            File.WriteAllText(path, content);
        }



        //deserialize an object
        public object retrieveObject(Type T, String source)
        { 
            String json = System.IO.File.ReadAllText(source); 
            object ob = new JavaScriptSerializer().Deserialize(json, T);
            return ob;
        }

        //append stuff to already written file
        public void append(String file, String new_content)
        {
            String time = DateTime.Now.ToString() ; 
            File.AppendAllText(file, time + "\n" + new_content); 
        }


        //move a file
        public void move(string from_path, String to_path, String file_name)
        { 
            if (!new DirectoryInfo(to_path).Exists)
            {
                Directory.CreateDirectory(to_path);
            } 
                //get file name that does not conflict with existing files in folder 
                String real_file_name = getNewName(to_path, file_name);

                //move file to destination
                String to = to_path + "\\" + real_file_name;
                String from = from_path + "\\" + file_name; 
                File.Move(from, to);
        }

        public void moveDir(string from_path, String to_path, String dir_name)
        { 
            if (!new DirectoryInfo(to_path).Exists)
            {
                Directory.CreateDirectory(to_path);
            } 

            //move file to destination
            String to = to_path + "\\" + dir_name;
            String from = from_path + "\\" + dir_name;

            try
            {
                Directory.Move(from, to);
            }
            catch (Exception es)
            {

            }
        }
         

         
        private String getNewName(String filePath, String name)
        {  
              //file may already exist, so we get name from this class specialized in renaming files
            Namer namer = new Namer(name, filePath);
            return namer.getName(); 
        
        }
           
        

         
    }
}
