using Mashup.Models;
using Newtonsoft.Json.Linq;

namespace Mashup.Services
{
    public class MusicBrainzService : IService
    {
        private readonly WikidataService _wikidataService;
        private readonly WikipediaService _wikipediaService;
        private readonly CoverArtService _coverArtService;
        public HttpClient HttpClient { get; }
        public string BaseUrl { get; } = "https://musicbrainz.org/ws/2/artist/";
        public string EndUrl { get; } = "?&fmt=json&inc=url-rels+release-groups";
        private readonly string _authUrl = "https://musicbrainz.org/oauth2/authorize";

        public MusicBrainzService(HttpClient httpClient, WikidataService wikidataService, WikipediaService wikipediaService, CoverArtService coverArtService)
        {
            HttpClient = httpClient;
            _wikidataService = wikidataService;
            _wikipediaService = wikipediaService;
            _coverArtService = coverArtService;
        }

        public async Task<Artist> GetArtist(string mbid)
        {
            try
            {
                await Auth();

                var response = await HttpClient.GetAsync(BaseUrl + mbid + EndUrl);

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var jsonContent = JObject.Parse(content);

                var albums = await GetAlbums(jsonContent, mbid);

                Artist artist = new()
                {
                    Albums = albums,
                    Mbid = mbid
                };

                var relations = (JArray)jsonContent["relations"];
                artist.Description = await GetArtistDescription(relations);
                return artist;
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException($"MBId = {mbid}. Error occurred while fetching artist. Did you provide a correct MBId? Error message: {e.Message}");
            }
            catch (Exception e)
            {
                throw new HttpRequestException($"MBId = {mbid}. Unexpected error occurred. Error message: {e.Message}");
            }
        }

        public async Task<string> GetArtistDescription(JArray relations)
        {
            try
            {
                string description = null;
                foreach (var relation in relations)
                {
                    var type = relation["type"]?.ToString();

                    if (type == "wikipedia")
                    {
                        var name = relation["url"]?["resource"]?.ToString();
                        description = await _wikipediaService.GetArtist(name);
                        break;
                    }

                    if (type == "wikidata")
                    {
                        var code = relation["url"]?["resource"]?.ToString();
                        var uri = new Uri(code);
                        var wikidataId = uri.Segments.Last();
                        description = await _wikidataService.GetArtist(wikidataId);
                        break;
                    }
                }
                return description ?? "No description found.";
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException($"There was a problem when fetching artist description. Error message: {e.Message}");
            }

        }

        public async Task<List<Album>> GetAlbums(JObject jsonContent, string mbid)
        {
            try
            {
                var albums = (JArray)jsonContent["release-groups"];
                List<Album> albumList = new();
                foreach (var album in albums)
                {
                    Album newAlbum = new()
                    {
                        Title = album["title"]?.ToString(),
                        Id = album["id"]?.ToString(),
                        Image = await _coverArtService.GetAlbumCover(album["id"]?.ToString())
                    };

                    albumList.Add(newAlbum);
                }

                return albumList;
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException($"There was a problem fetching albums. Error message: {e.Message}");
            }
        }

        public async Task Auth()
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.local.json")
                .Build();

                string clientId = configuration["OAuthSettings:ClientId"];
                string clientSecret = configuration["OAuthSettings:ClientSecret"];

                HttpClient.DefaultRequestHeaders.Add("Client_Id", clientId);
                HttpClient.DefaultRequestHeaders.Add("Client_Secret", clientSecret);

                await HttpClient.GetStringAsync(_authUrl);
                HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                HttpClient.DefaultRequestHeaders.Add("User-Agent", "Mashup/1.0");

            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException($"There was a problem authenticating to MusicBrainz. Error message: {e.Message}");
            }
        }
    }
}
