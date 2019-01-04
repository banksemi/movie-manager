using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager
{
    public enum AudioType
    {
        ALL, AAC, AC3, DTS, FLAC, ETC
    }
    public class AudioSetting
    {
        public AudioType OriginalCodec;
        public AudioType TargetCodec;

        public double VolumeBoost = 1;
    }
}
