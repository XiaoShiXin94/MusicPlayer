using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music
{
    class SongsJson
    {
        public string msg { get; set; }
        public string code { get; set; }
        public struct data
        {
            public string songId { get; set; }
            public string songName { get; set; }
            public string songUrl { get; set; }
            public string singer { get; set; }
            public string picture { get; set; }
        }
        public data songsData;
    }
}
