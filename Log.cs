using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarisoftCleanDesktop
{
    //helps make logs
    class Log
    {
        public environment mode {get; set;} 
        private static Log instance;

        public static Log getInstance()
        {
            if (instance == null)
            {
                instance = new Log();
            }
            instance.mode = environment.development;

            return instance;
        }




        //this is a singleton class. cannot be instantiated
        private Log(){ }



        //print debug information to screen
        public void i(String message)
        {
            if (mode == environment.production)
            {
                IO.getInstance().append(Preferences.LogStorage, message);
            }
            if (mode == environment.development)
            {
                Console.WriteLine(message);
            }
            
        }
    }
}
