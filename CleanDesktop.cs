using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LarisoftCleanDesktop
{
    class CleanDesktop :IO_Listener
    {
        //change this directory to any other directory and the app stops cleaning the desktop and starts cleaning that folder
        String DefaultPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        //class for holding all extensions and their mapped directories together
        Folders folders;

        //class for logging
        static Log log;

        //class for mopping up files
        Cleaner cleaner;

        //class for file operations
        IO input_output;

        //class for all console outputs
        Speaker speaker;
    
        
        public CleanDesktop(){

            cleaner = Cleaner.getInstance();
            cleaner.DefaultPath = DefaultPath;

            input_output = IO.getInstance();
            input_output.addObserver(this);

            speaker = Speaker.getInstance();

            log = Log.getInstance();

            FileSystemWatcher watcher = new FileSystemWatcher();
             


            //PLACE SHORTCUT ON USER DESKTOP 
            //exe location of this app
            String startUp = Assembly.GetEntryAssembly().Location;


            //@Todo: does this really have to run every time user starts app? Pros: IT will always be on startup folder, Cons: It may get annoying
            //@Todo: Changing this behavior is as simple as moving the add_shortcut command into the isfirsttime method
            ShortCutMaker sm = new ShortCutMaker();
            sm.add_desktop_shortcut(Environment.GetFolderPath(Environment.SpecialFolder.Startup), startUp, "CleanDesktop", "Launch larisoft Clean Desktop");



            BackgroundWorker bg = new BackgroundWorker();
           
            bg.DoWork += (object obj, DoWorkEventArgs d) =>
            {
                if (Preferences.isFirstTimeRunning() == true)
                {
                    cleaner.CreateDefaultExtensions();
                }

                folders = (Folders)input_output.retrieveObject(typeof(Folders), Preferences.ExtensionListStorage);


            };

            bg.RunWorkerCompleted += (object ob, RunWorkerCompletedEventArgs de) =>
            {

                //START UP OPERATIONS
                watcher.Path = cleaner.DefaultPath;
                watcher.EnableRaisingEvents = true;
                watcher.Created += new FileSystemEventHandler(watcher_Created);
                watcher.Deleted += new FileSystemEventHandler(watcher_Deleted);
                watcher.Changed += new FileSystemEventHandler(watcher_Changed);
                watcher.Renamed += new RenamedEventHandler(watcher_Renamed);
                cleaner.invalidate(folders);

                speak("CleanDesktop is Running");
                speak("Copyright larisoft 2016, larypetero@gmail.com");
            };

            bg.RunWorkerAsync();
            speak("CleanDesktop is loading Extensions..."); 
            do_not_close();

        }

        //IO Listener reports new object
        public void objectsChanged(object objects)
        {
            folders = (Folders)objects;
        }

        private void watcher_Renamed(object sender, RenamedEventArgs e)
        {
            speak(e.OldName + " is now: " + e.Name);
            FileSystemEventArgs es = new FileSystemEventArgs(WatcherChangeTypes.Created, e.Name, e.Name);
            watcher_Created(null, es);
        }

        private void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            speak(e.Name + " has changed");
        }

        private void watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            speak(e.Name + " file has been deleted");
        }

        private void watcher_Created(object sender, FileSystemEventArgs e)
        {
            speak(e.Name + " has been created ");
            
            //check if file really exists and then move it. 
            //@Todo is this check necessary? 
            FileInfo inf = new FileInfo(cleaner.DefaultPath + "\\" + e.Name); 

            if (inf.Exists && !(inf.Name.Equals("CleanDesktop.lnk")) && !inf.IsReadOnly)
            {   
                cleaner.move_to_right_folder(inf.Name, inf.Extension, folders);
                speaker.speak(" Moved " + inf.Name);
            } 

        }




        private void do_not_close()
        {
            String command = "";
            Boolean run = true;
            while (run)
            {
                command = Console.ReadLine().ToLower().Trim();
                if (command.ToLower().StartsWith("add"))
                {
                    if (validate_command(command))
                    {
                        add_custom_extension(command);
                    }
                    else
                    {
                        speak("incorrect command format");
                        speak("type 'add .extension to folder'");
                        speak("To add a custom extension mapping type");
                        speak("e.g.");
                        speak("add .jpg to Pictures");
                    }
                }
                else if (command.ToLower().StartsWith("list"))
                {
                    cleaner.listExtensions(folders);
                }
                else if (command.ToLower().StartsWith("exit"))
                {

                    run = false;
                }
                else
                {
                    help_message();
                }
            
        }}





        private Boolean validate_command(String command)
        {
            command = command.ToLower();
            
            if (!command.Contains("to")) return false;
             
            String[] potentialParams = command.Replace("to", "").Replace("add","").Trim().Split(new String[]{" "}, StringSplitOptions.RemoveEmptyEntries);
            
            //if it contains any other words than folder and extension return false
            if(potentialParams.Length != 2) return false;

            //if first parameter doesnt start with "." return false
            if (!potentialParams[0].StartsWith(".")) return false;

            return true;

        }

        private void add_custom_extension(String statement)
        {
             
            statement = statement.ToLower().Trim();

            if (statement.StartsWith("add"))
            {
                String[] param = (statement.Replace("add", "").Replace("to", "").Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries));
                AddExtension n_extension = new AddExtension(param);
                input_output.saveObject(n_extension.getNewExtensions(), Preferences.ExtensionListStorage);
            }
        }

        private void help_message()
        {

            speak("Type 'exit' + Enter to Quit");
            speak("Type 'list' + Enter to See all extensions and their folders");
            speak("Type 'add .ext to folder' to add custom extension");

        }

        private void speak(String message)
        {
            Speaker.getInstance().speak(message);
                    
        }
    
    }
}
