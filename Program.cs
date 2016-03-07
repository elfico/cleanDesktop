
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IWshRuntimeLibrary;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Web.Script.Serialization;
using System.Threading;
using System.ComponentModel;

namespace LarisoftCleanDesktop
{
    class Program
    {
        static void Main(string[] args)
        {
            //Start Application
            try
            {   Console.WindowHeight = 5; 
                CleanDesktop app = new CleanDesktop();
                
            }
            catch (Exception e)
            {
                Main(null);
            }
        }
    }

}
