using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
namespace MovieManager
{
    public class ffmpeg
    {
        private static string FFMPEG = Config.FFMPEGPath.FullName + "ffmpeg.exe";
        private static string FFPROBE = Config.FFMPEGPath.FullName + "ffprobe.exe";
        public VideoType VideoCodec { get; private set; }
        public AudioType AudioCodec { get; private set; }
        public string State { get; private set; }
        public FileInfo file { get; private set; }
        public ffmpeg(FileInfo file)
        {
            State = null;
            if (file.Exists == false)
                throw new FileNotFoundException();
            this.file = file;
            GetInfo();
        }
        private void GetInfo()
        {
            ProcessStartInfo psiProcInfo = new ProcessStartInfo();
            psiProcInfo.FileName = FFPROBE;
            psiProcInfo.Arguments = "-v error -show_entries stream=codec_type,codec_name -of default=noprint_wrappers=1:nokey=1" + " \"" + file.FullName + "\"";
            psiProcInfo.UseShellExecute = false;
            psiProcInfo.RedirectStandardOutput = true;
            Process temp = new Process();
            temp.StartInfo = psiProcInfo;
            temp.Start();
            StreamReader reader = temp.StandardOutput;
            string codec;
            while ((codec = reader.ReadLine()) != null)
            {
                string type = reader.ReadLine();
                if (type == "video") VideoCodec = VideoType.ETC;
                if (type == "audio")
                {
                    AudioCodec = AudioType.ETC;
                    if (codec == "ac3") AudioCodec = AudioType.AC3;
                    if (codec == "aac") AudioCodec = AudioType.AAC;
                    break; // Only check first audio stream
                }
            }
        }
        public bool Convert(VideoSetting videoSetting,AudioSetting audioSetting, FileInfo newPath)
        {
            string vcodec = "copy";
            string acodec = "copy";

            ProcessStartInfo psiProcInfo = new ProcessStartInfo();
            psiProcInfo.FileName = FFMPEG;
            psiProcInfo.Arguments = " -i \"" + file.FullName + "\"";

            if (videoSetting != null) vcodec = videoSetting.TargetCodec.ToString().ToLower();
            if (audioSetting != null)
            {
                if (audioSetting.VolumeBoost == 1)
                {
                    if (audioSetting.TargetCodec != AudioCodec) // Note: Do not combine with the above if statement.
                        acodec = audioSetting.TargetCodec.ToString().ToLower();
                }
                else
                {
                    acodec = audioSetting.TargetCodec.ToString().ToLower();
                    psiProcInfo.Arguments += " -af \"volume = " + audioSetting.VolumeBoost + "\"";
                }
            }

            psiProcInfo.Arguments += " -vcodec " + vcodec + " -acodec " + acodec;
            psiProcInfo.Arguments += " \"" + newPath.FullName + "\"";
            psiProcInfo.UseShellExecute = false;
            psiProcInfo.RedirectStandardError = true;

            Process temp = new Process();
            temp.StartInfo = psiProcInfo;
            temp.Start();
            StreamReader reader = temp.StandardError;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.IndexOf("frame=") == 0)
                {
                    State = line;
                }
            }
            return State != null;
        }
    }
}
