using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace MovieManager
{
    public class MessageItem
    {
        private string type;
        private string message;
        public MessageItem(string type, string message)
        {
            Update(type, message);
        }
        public void Update(string type, string message)
        {
            this.type = type;
            this.message = message;
            ConsoleSystem.ConsoleUpdate();
        }
        public override string ToString()
        {
            return "[" + type + "] " + message;
        }
    }

    public static class ConsoleSystem
    {
        private static List<MessageItem> Console_Message = new List<MessageItem>();
        public static void ConsoleUpdateThread()
        {
            while (true)
            {
                ConsoleUpdate();
                Thread.Sleep(1000);
            }
        }
        public static void AddMessage(MessageItem item)
        {
            if (Console_Message.Count >= Config.MaxConsoleLine)
                Console_Message.RemoveAt(0);
            Console_Message.Add(item);
            ConsoleUpdate();
        }
        public static void ConsoleUpdate()
        {
            lock (ThreadList.threads)
            {
                Console.Clear();
                int i = 0;
                foreach (ffmpeg ffmpeg in ThreadList.threads)
                {
                    Console.WriteLine("#{0} " + ffmpeg.file.FullName, i++);
                    Console.WriteLine("\tState : " + ffmpeg.State);
                    Console.WriteLine();
                }
            }
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("  Massage");
            Console.WriteLine("-------------------------------------------------------");

            foreach (MessageItem Message in Console_Message)
            {
                Console.WriteLine(Message);
            }
        }
    }
}
