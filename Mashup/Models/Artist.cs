namespace Mashup.Models
{
    public class Artist
    {
        public Artist()
        {
            Albums = new List<Album>();
        }

        public string Mbid { get; set; }
        public string Description { get; set; }
        public List<Album> Albums { get; set; }

    }
}
