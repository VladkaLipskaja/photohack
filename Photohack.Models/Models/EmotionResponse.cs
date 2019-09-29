namespace Photohack.Models
{
    public class EmotionResponse
    {
        public EmotionStatistics Emotion { get; set; }

        public class EmotionStatistics
        {
            public decimal Happy { get; set; }
            public decimal Sad { get; set; }
            public decimal Angry { get; set; }
            public decimal Fear { get; set; }
            public decimal Excited { get; set; }
            public decimal Indifferent { get; set; }
        }
    }
}
