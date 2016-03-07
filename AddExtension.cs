using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.IO;
namespace LarisoftCleanDesktop
{
    class AddExtension
    {
        Folders ExistingFolders;
        String[] param;
        public AddExtension(String[] param){

            this.param = param;
        }

        public Folders getNewExtensions()
        {

            log(param[0] + " to " + param[1]);

            ExistingFolders = getAllExtensions();

            Folder_Item item = getFolderItem(param[1]);
            item.addExtension(param[0]);

            ExistingFolders.items.Add(item); 
             
            log(param[0] + " mapped to " + param[1]);
             
            return ExistingFolders;
            
        }


        private Boolean extensionAlreadyExists( String extension){

           return ExistingFolders.items.Count(p => p.ownsExtension(extension)) > 0;
        }


        private Folder_Item getFolderItem(String folder)
        {

            if (ExistingFolders.items.Count(i => i.Folder.Equals(folder)) > 0)
            {
                return ExistingFolders.items.Last(l => l.Folder.Equals(folder));
            }

            else
            { 
                Directory.CreateDirectory(Preferences.Desktop+"\\"+folder);

                return new Folder_Item { Folder = folder };
            }
        
        }

        private Folders getAllExtensions()
        {
           String json = System.IO.File.ReadAllText(Preferences.getStorage() + "\\" + Preferences.ExtensionListFileName);
           Folders folders = (Folders)new JavaScriptSerializer().Deserialize(json, typeof(Folders));
           return folders;
        }


        private void log(String message)
        {
            Speaker.getInstance().speak(message);
        }
    }
}
