using System.Collections.Generic;

namespace Photohack.Models
{
    public static class PhotoEmotionTemplateConfigs
    {
        public static readonly int Default = 2708;

        public static readonly Dictionary<int, int> EmotionFirstTemplate = new Dictionary<int, int>
        {
            {(int)EmotionEnum.Happy, 1630 },
            {(int)EmotionEnum.Sad, 1629 },
            {(int)EmotionEnum.Angry, 2085 },
            {(int)EmotionEnum.Excited, 2091 }
        };

        public static readonly Dictionary<int, int> EmotionLastTemplate = new Dictionary<int, int>
        {
            {(int)EmotionEnum.Angry, 1191},
            {(int)EmotionEnum.Sad, 2337 },
            {(int)EmotionEnum.Excited, 410 }
        };
    }
}
