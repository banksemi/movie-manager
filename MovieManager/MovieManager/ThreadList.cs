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
        public static List<ffmpeg> threads = new List<ffmpeg>();
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
    }
}
