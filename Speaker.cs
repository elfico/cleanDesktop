using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarisoftCleanDesktop
{   //this object helps output stuff to the console 
    class Speaker
    {   
        private static Speaker instance;

        private Speaker() { }

        public static Speaker getInstance()
        {
            if (instance == null)
            {
                instance = new Speaker();
            }

            return instance;
        }


        public void speak(String message)
        {
            Console.WriteLine(message);
        }
    }
}
