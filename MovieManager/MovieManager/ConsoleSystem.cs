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

        private static string console = "";
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
            StringBuilder stdout = new StringBuilder();
            lock (ThreadList.threads)
            {
                int i = 0;
                foreach (ffmpeg ffmpeg in ThreadList.threads)
                {
                    stdout.AppendLine(string.Format("#{0} " + ffmpeg.file.FullName, i++));
                    stdout.AppendLine("\tState : " + ffmpeg.State);
                    stdout.AppendLine();
                }
            }
            stdout.AppendLine("-------------------------------------------------------");
            stdout.AppendLine("  Message");
            stdout.AppendLine("-------------------------------------------------------");

            foreach (MessageItem Message in Console_Message)
            {
                stdout.AppendLine(Message.ToString());
            }
            if (!console.Equals(stdout.ToString()))
            {
                console = stdout.ToString();
                Console.Clear();
                Console.Write(console);
            }
        }
    }
}
