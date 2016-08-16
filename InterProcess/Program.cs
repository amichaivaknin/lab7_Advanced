using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace SyncDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Mutex syncFileMutex = new Mutex(false, "MySyncMutex");
            
            for (var i = 0; i < 10000; i++)
            {
                syncFileMutex.WaitOne();
                Console.WriteLine("Mutex lock");
                try
                {
                    var streamWriter = new StreamWriter(@"c:\temp\data.txt");
                    streamWriter.WriteLine(Process.GetCurrentProcess().Id);
                    streamWriter.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
                finally
                { 
                    syncFileMutex.ReleaseMutex();
                    Console.WriteLine("Mutex unlock");
                }  
            }

            syncFileMutex.Dispose();
        }
    }
}
