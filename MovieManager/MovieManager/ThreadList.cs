using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace MovieManager
{
    public static class ThreadList
    {
        private static List<ffmpeg> threads = new List<ffmpeg>();
        public static int Count => threads.Count;
        public static void Add(ffmpeg ffmpeg, Action action)
        {
            Thread thread = null;
            thread = new Thread(
                delegate () {
                    action.Invoke();

                    // Dispose Event
                    lock (threads)
                    {
                        threads.Remove(ffmpeg);
                    }
                });

            // Start Event
            lock(threads)
            {
                threads.Add(ffmpeg);
                thread.Start();
            }
        }
        public static void ConsoleUpdateThread()
        {
            while (true)
            {
                lock(threads)
                {
                    Console.Clear();
                    int i = 0;
                    foreach(ffmpeg ffmpeg in threads)
                    {
                        Console.WriteLine("#{0} " +  ffmpeg.file.FullName,i++);
                        Console.WriteLine("\tState : " + ffmpeg.State);
                        Console.WriteLine();
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
}
