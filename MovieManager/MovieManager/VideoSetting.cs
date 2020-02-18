using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager
{
    public enum VideoType
    {
        ALL, H264, HEVC, ETC, HEVC_NVENC
    }
    public class VideoSetting
    {
        public VideoType OriginalCodec;
        public VideoType TargetCodec;
        public string option;
    }
}
