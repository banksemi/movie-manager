using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager
{
    public enum VideoType
    {
        ALL, ETC
    }
    public class VideoSetting
    {
        public VideoType OriginalCodec;
        public VideoType TargetCodec;
    }
}
