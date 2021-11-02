using System;
using System.IO;
using System.Diagnostics;

using System.Timers;

namespace ProcessAuxLib
{
    public static class ProcessAux
    {
        public static float MakeProcess(ref ProcessStartInfo ProcessObj)
        {
            try
            {
                //Start timer
                Timer t = new Timer();
                t.Start();

                //Start process
                Process.Start(ProcessObj);

                //Stop timer
                t.Stop();

                double d = t.Interval;

                return (float)d;
            }
            catch(Exception)
            {
                return -1.0f;
            }
        }
    }
}
