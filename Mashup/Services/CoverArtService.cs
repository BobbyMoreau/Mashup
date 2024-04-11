using Newtonsoft.Json.Linq;
using System.Net;

namespace Mashup.Services
{
    public class CoverArtService : IService
    {
        public HttpClient HttpClient { get; }
        public string BaseUrl { get; } = "https://coverartarchive.org/release-group/";
        public string EndUrl { get; }

        public CoverArtService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<string> GetAlbumCover(string mbid)
        {
            try
            {
                var response = await HttpClient.GetAsync(BaseUrl + mbid);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var jsonContent = JObject.Parse(content);
                    var image = jsonContent["images"]?.FirstOrDefault()?["image"]?.ToString() ?? "No cover available";

                    return image;
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    //if album has an id but not a coverArt
                    return "No cover available";
                }
                else
                {
                    throw new HttpRequestException($"Status code: {response.StatusCode}");
                }

            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException($"There was a problem fetching the cover. Error message: {e.Message}");
            }
        }
    }
}
