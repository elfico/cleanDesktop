using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.Web.Script.Serialization;
using System.IO;

namespace LarisoftCleanDesktop
{   
    [Serializable]
    class Folders
    {
        //a class to help keep all folder_items and their extensions in one object. Nothing else
        public Folders()
        {

        }

        public List<Folder_Item> items { get; set; }
         


    }
}
