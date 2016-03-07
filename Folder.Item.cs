using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarisoftCleanDesktop
{
    //this object keeps a one to many relationship between a folder and numerous extensions.
    [Serializable]
    class Folder_Item
    {

        public Folder_Item(){

        }

        public List<String> Extensions { get; set; }
        public String Folder { get; set; }

        public Folder_Item(String name)
        {
            this.Folder = name;
        }


        public void addExtensions(String[] s)
        {
            if (this.Extensions == null)
            {
                this.Extensions = new List<String>();
            }
            foreach (String str in s)
            { 
                Extensions.Add(str);
            }
        }

        public void addExtension(String s)
        { 
            if (this.Extensions == null)
            {
                this.Extensions = new List<String>();
            }
            Extensions.Add(s);
        }


        public String toString()
        {
            if (this.Extensions == null)
            {
                return "No extensions";
            }

            StringBuilder build = new StringBuilder();

            foreach (String s in Extensions)
            {
                build.Append(s + ", ");
            }

            return build.ToString();

        }
        public Boolean ownsExtension(String extension)
        {
            //if this is the others folder, it has no extension
            if (Folder.Equals("Others"))
            {
                return false;
            }
            return (Extensions.Contains(extension.ToLower()));
        }
    }
}
