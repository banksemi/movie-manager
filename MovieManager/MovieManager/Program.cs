using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
namespace MovieManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Config.Setup();
            if (Config.TempDirectory.Exists == false) Config.TempDirectory.Create();
            new Thread(ConsoleSystem.ConsoleUpdateThread).Start();
            // Check New File in Main Thread
            while(true)
            {
                if (Config.TargetDirectory.Exists)
                {
                    List<FileInfo> FileData = FileIO.DirSearch(Config.TargetDirectory);
                    foreach (var item in FileData)
                    {
                        // Wait idle thread bacause of MaxThreadCount.
                        while (Config.MaxThread <= ThreadList.Count) Thread.Sleep(1000);
                        if (Config.ReadFileExtension.Contains(item.Extension.Substring(1)))
                        {
                            FileInfo OutputFile = FileIO.Replace_Extension(item, Config.OutputFileExtension);
                            FileInfo TempFile = FileIO.Replace_Directory(OutputFile, Config.TempDirectory);
                            FileInfo LockFile = FileIO.Replace_Extension(item, "lock");
                            
                            if (OutputFile.Exists) continue; // If the target file already exists
                            if (TempFile.Exists) continue;  // If a file with the same name exists in the temporary folder
                            if (LockFile.Exists) continue; // If another computer perform converting file? (Only 1 thread)


                            // Check Setting List

                            ffmpeg ffmpeg = new ffmpeg(item);
                            VideoSetting videoSetting = null;
                            AudioSetting audioSetting = null;
                            foreach (VideoSetting checkset in Config.VideoSettings)
                            {
                                if (checkset.OriginalCodec == VideoType.ALL ||
                                    checkset.OriginalCodec == ffmpeg.VideoCodec)
                                    videoSetting = checkset;
                            }
                            foreach (AudioSetting checkset in Config.AudioSettings)
                            {
                                if (checkset.OriginalCodec == AudioType.ALL ||
                                    checkset.OriginalCodec == ffmpeg.AudioCodec)
                                    audioSetting = checkset;
                            }

                            if (videoSetting != null || audioSetting != null)
                            {
                                ThreadList.Add(ffmpeg, delegate()
                                {
                                    try
                                    {
                                        MessageItem Message = new MessageItem("Found File", item.Name);
                                        ConsoleSystem.AddMessage(Message);
                                        // Create Lock File
                                        FileIO.CreateFile(LockFile);
                                        Message.Update("Create Lock", LockFile.Name);
                                        Message.Update("Convert File", item.Name + " -> " + TempFile.Name);
                                        ffmpeg.Convert(videoSetting, audioSetting, TempFile);
                                        Message.Update("Move File", TempFile.Name + " -> " + OutputFile.Name);
                                        TempFile.MoveTo(OutputFile.FullName);
                                        Message.Update("Delete File", item.Name);
                                        item.Delete();
                                        Message.Update("Complete", item.Name);
                                    }
                                    catch (Exception e)
                                    {
                                        if (TempFile.Exists) TempFile.Delete();
                                    }
                                    finally
                                    {
                                        if (LockFile.Exists) LockFile.Delete();
                                    }
                                });
                            }


                        }
                    }

                    Thread.Sleep(Config.ResearchTime * 1000);
                }
            }
        }
    }
}
