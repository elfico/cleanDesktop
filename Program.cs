using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LarisoftCleanDesktop
{
    class Program
    {
        static String Desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        static void Main(string[] args)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();

            watcher.Path = Desktop;
            watcher.EnableRaisingEvents = true;
            watcher.Created += new FileSystemEventHandler(watcher_Created);
            watcher.Deleted += new FileSystemEventHandler(watcher_Deleted);
            watcher.Changed += new FileSystemEventHandler(watcher_Changed);
            watcher.Renamed += new RenamedEventHandler(watcher_Renamed);

            Console.WriteLine("larisoft Clean Desktop is on!!!");
            clean_up_desktop();

            Console.WriteLine("copyright larisoft");
            do_not_close();

        }


        static void clean_up_desktop()
        {

            FileInfo[] files = new DirectoryInfo(Desktop).GetFiles();

            foreach (FileInfo f in files)
            {
                if (f.Exists)
                {
                    move_to_right_folder(f.Name, f.Extension);
                }

            }
        }

        static void watcher_Renamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine(e.OldName + " is now: " + e.Name);
            FileSystemEventArgs es = new FileSystemEventArgs(WatcherChangeTypes.Created, e.Name, e.Name);
            watcher_Created(null, es);
        }

        static void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(e.Name + " has changed");
        }

        static void watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(e.Name + " file has been deleted");
        }

        static void watcher_Created(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(e.Name + " file has been created.");

            FileInfo inf = new FileInfo(Desktop + "\\" + e.Name);

            if (inf.Exists)
            {
                log("a file created");

                move_to_right_folder(inf.Name, inf.Extension);
            }
            else
            {
                log("not a file created");
            }

        }

        static void move_to_right_folder(String fileName, String extension)
        {
            log("File name is " + fileName + " and extension is " + extension);
            extension = extension.ToLower();

            if (extension.Equals(".doc") || extension.Equals(".docx") || extension.Equals(".pdf"))
            {

                move(Desktop + "\\Documents", fileName);
            }
            else if (extension.Equals(".avi") || extension.Equals(".vob") || extension.Equals(".mp4"))
            {


                move(Desktop + "\\Videos", fileName);

            }

            else if (extension.Equals(".flac") || extension.Equals(".wav") || extension.Equals(".mp3"))
            {


                move(Desktop + "\\Music", fileName);

            }

            else if (extension.Equals(".lnk") || extension.Equals(".exe") || extension.Equals(".jar") || extension.Equals(".apk"))
            {


                move(Desktop + "\\Applications", fileName);

            }
            else if (extension.Equals(".jpg") || extension.Equals(".png") || extension.Equals(".gif"))
            {


                move(Desktop + "\\Pictures", fileName);

            }

            else if (extension.Equals(".js") || extension.Equals(".bat") || extension.Equals(".py"))
            {


                move(Desktop + "\\Scripts", fileName);

            }

            else if (extension.Equals(".txt"))
            {


                move(Desktop + "\\TextFiles", fileName);

            }

            else if (extension.Equals(".cdr"))
            {


                move(Desktop + "\\Design", fileName);

            }

            else
            {

                move(Desktop + "\\Others", fileName);
            }

        }


        static void move(String correct_path, string file_name)
        {

            if (!new DirectoryInfo(correct_path).Exists)
            {
                Directory.CreateDirectory(correct_path);
            }

            try
            {
                File.Move(Desktop + "\\" + file_name, correct_path + "\\" + file_name);
            }
            catch (Exception e)
            {
                log("unable to move " + e.Message);
            }
        }

        static void do_not_close()
        {
            String command = "";
            while (!command.Contains("Exit"))
            {
                command = Console.ReadLine().ToLower().Trim();
            }
        }


        static void log(String message)
        {
            Console.WriteLine(message);
        }
    }

}
