using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace MovieManager
{
    public static class Config
    {
        /// <summary>
        /// How many files will be encoded at the same time?
        /// </summary>
        public static int MaxThread = 2;

        public static string[] ReadFileExtension = { "mkv" };
        public static string OutputFileExtension = "mp4";
        public static DirectoryInfo FFMPEGPath = new DirectoryInfo("ffmpeg-20200216-8578433-win64-static\\bin\\");

        public static DirectoryInfo TargetDirectory = new DirectoryInfo("video");

        public static int EncoderMinResolution = 1080;
        /// <summary>
        /// Temporary folder that stores the file being encoded.
        /// <para> I recommend 16GB or more. </para>
        /// <para> Default is [Application Path] </para>
        /// </summary>
        public static DirectoryInfo TempDirectory = new DirectoryInfo("temp");

        /// <summary>
        /// When there are no more new files, the program has an idle time. (Seconds)
        /// </summary>
        public static int ResearchTime = 600;

        public static int MaxConsoleLine = 10;

        public static List<AudioSetting> AudioSettings = new List<AudioSetting>();
        public static List<VideoSetting> VideoSettings = new List<VideoSetting>();
        public static void Setup()
        {
            /*
            AudioSettings.Add(new AudioSetting()
            {
                OriginalCodec = AudioType.ALL,
                TargetCodec = AudioType.AAC,
                VolumeBoost = 1.15
            });
            */

            VideoSettings.Add(new VideoSetting()
            {
                OriginalCodec = VideoType.H264,
                TargetCodec = VideoType.HEVC_NVENC,
                // -profile:v main -preset medium -b:v 2000k -rc vbr_hq -rc-lookahead 30 -spatial_aq 1 -aq-strength 10 -refs 4
                option = "-profile:v main -preset slow -b:v 2200k -rc vbr_hq -rc-lookahead 30 -spatial_aq 1 -aq-strength 10 -refs 4"
            });

            TempDirectory = new DirectoryInfo("V:\\Temp\\");
            TargetDirectory = new DirectoryInfo("V:\\");

        }
    }
}
