using System.Collections.Generic;

namespace Photohack.Models
{
    public class MusicTrack
    {
        public List<Track> Data { get; set; }

        public class Track
        {
            public long Id { get; set; }

            public string Title { get; set; }

            public string Preview { get; set; }

            public ArtistInfo Artist { get; set; }

            public class ArtistInfo
            {
                public long Id { get; set; }

                public string Name { get; set; }

                public string Picture { get; set; }
            }
        }
    }
}
