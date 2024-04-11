using Newtonsoft.Json.Linq;

namespace Mashup.Services
{
    public class WikidataService : IService
    {
        private readonly WikipediaService _wikipediaService;
        public HttpClient HttpClient { get; }
        public string BaseUrl { get; } = "https://www.wikidata.org/w/api.php?action=wbgetentities&ids=";
        public string EndUrl { get; } = "&format=json&props=sitelinks";

        public WikidataService(HttpClient httpClient, WikipediaService wikipediaService)
        {
            HttpClient = httpClient;
            _wikipediaService = wikipediaService;
        }

        public async Task<string> GetArtist(string wikidataId)
        {
            try
            {
                var response = await HttpClient.GetAsync(BaseUrl + wikidataId + EndUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var wikiLink = GetWikipediaLink(content, wikidataId);
                var encodedTitle = Uri.EscapeDataString(wikiLink);
                var description = await _wikipediaService.GetArtist(encodedTitle);

                return description;
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException($"There was an issue fetching the description from Wikipedia. Error message: {e.Message}");
            }
        }

        public string GetWikipediaLink(string content, string wikidataId)
        {
            try
            {
                var jsonContent = JObject.Parse(content);
                var siteLinks = (JObject)jsonContent["entities"]?[wikidataId]?["sitelinks"];
                var title = siteLinks["enwiki"]?["title"]?.ToString();

                return title;
            }
            catch (Exception e)
            {
                throw new Exception($"There was an issue fetching the description from Wikipedia. Exception: {e.Message}");
            }

        }
    }
}
