using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarisoftCleanDesktop
{
    //sort of the engine of the app. creates, moves, lists extensions
    class Cleaner
    {
        private static Cleaner instance;
        private Cleaner() { }

        //this is the file that is been watched
        public String DefaultPath { get; set; }


        public static Cleaner getInstance()
        {
            if (instance == null)
            {
                instance = new Cleaner();
            }
            return instance;
        }

        public void CreateDefaultExtensions()
        {
            //all others  
            Folder_Item others = new Folder_Item("Others");


            //Pictures
            Folder_Item pictures = new Folder_Item("Pictures");
            pictures.addExtensions(new String[] { ".jpg", ".png", ".ico", ".jpeg" });


            //Music 
            Folder_Item Music = new Folder_Item("Music");
            Music.addExtensions(new String[] { ".mp3", ".wav", ".flac" });


            //Applications 
            Folder_Item applications = new Folder_Item("Applications");
            applications.addExtensions(new String[] { ".exe", ".apk", ".jar", ".lnk" });


            //Videos
            Folder_Item vids = new Folder_Item("Videos");
            vids.addExtensions(new String[] { ".vob", ".mp4", ".avi", ".mkv", ".flv" });


            //Scripts  
            Folder_Item scripts = new Folder_Item("Scripts");
            scripts.addExtensions(new String[] { ".js", ".py", ".css", ".bat" });


            //Documents 
            Folder_Item documents = new Folder_Item("Documents");
            documents.addExtensions(new String[] { ".doc", ".docx", ".pdf" });

            //Web HTML Files
            Folder_Item web_design = new Folder_Item("HTML"); 
            web_design.addExtensions(new String[] { ".html", ".htm" });


            //TextFiles
            Folder_Item text = new Folder_Item("TextFiles");
            text.addExtensions(new String[] { ".txt" });



            Folders all = new Folders();
            all.items = new List<Folder_Item>(new Folder_Item[] { others, pictures, Music, applications, vids, scripts, documents, web_design, text });

            IO.getInstance().saveObject(all, Preferences.ExtensionListStorage);

        }




        public void invalidate(Folders folders)
        {
            FileInfo[] files = new DirectoryInfo(DefaultPath).GetFiles();

            foreach (FileInfo f in files)
            {
                if (f.Exists && !(f.Name.Equals("CleanDesktop.lnk")))
                {
                    move_to_right_folder(f.Name, f.Extension, folders);
                }

            }
        }



        public void move_to_right_folder(String fileName, String extension, Folders folders)
        {
            if (extension.Equals(".html") || extension.Equals(".htm"))
            {
                move_special(fileName, extension, folders);
            }
            else
            {

                Boolean moved = false;

                for (int i = folders.items.Count - 1; i > (-1); i--)
                {
                    Folder_Item d = folders.items[i];
                    if (d.ownsExtension(extension))
                    {
                        Speaker.getInstance().speak("Moved" + fileName + " to " + d.Folder);
                        try
                        {
                            IO.getInstance().move(DefaultPath, DefaultPath + "\\" + d.Folder, fileName);
                            moved = true;
                            break;
                        }
                        catch (Exception es)
                        {
                            moved = true;
                        }
                    }
                }

                if (!moved)
                {
                    Folder_Item Item = folders.items.Single(d => d.Folder.Equals("Others"));
                    IO.getInstance().move(DefaultPath, DefaultPath + "\\" + Item.Folder, fileName);


                }
            }

        }


        private void move_special(String fileName, String extension, Folders folders)
        {
                SpecialFile sf = new SpecialFile(DefaultPath + "\\" + fileName);

                for (int i = folders.items.Count - 1; i > (-1); i--)
                {
                    Folder_Item d = folders.items[i];
                    if (d.ownsExtension(extension))
                    {
                        String belongings = sf.getBelongingsFolder();

                        Console.WriteLine("Dir Name "  + belongings);
                        if (belongings != null)
                        {
                            IO.getInstance().moveDir(DefaultPath, DefaultPath + "\\" + d.Folder, belongings);
                        }

                        Speaker.getInstance().speak("Moved" + fileName + " to " + d.Folder);

                        IO.getInstance().move(DefaultPath, DefaultPath + "\\" + d.Folder, fileName);

                        break;

                    }
                    
                } 

        }


        public void listExtensions(Folders items)
        {
            

            for (int i = items.items.Count-1; i > (-1); i--)
            {
                Folder_Item this_item = items.items[i];

                
                speak(this_item.Folder + "\n"+this_item.toString());
                speak("");
            }

            speak("Items are listed in the order which the system searches through them");


        }

        public void speak(String Message = "CopyRight larisoft2016"){

            Speaker.getInstance().speak(Message);
        }
    }
}
