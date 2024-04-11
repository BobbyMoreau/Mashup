using Newtonsoft.Json.Linq;

namespace Mashup.Services
{
    public class WikipediaService :IService
    {
        public HttpClient HttpClient { get; }
        public string BaseUrl { get; } = "https://en.wikipedia.org/w/api.php?action=query&format=json&prop=extracts&exintro&explaintext&titles=";
        public string EndUrl { get;}

        public WikipediaService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<string> GetArtist(string name)
        {
            try
            {
                var response = await HttpClient.GetAsync(BaseUrl + name);
                
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                var jsonObject = JObject.Parse(content);

                var page = jsonObject["query"]?["pages"]?.Values().FirstOrDefault();
               
                return page["extract"]?.ToString();

            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException($"There was an issue fetching the description. Exception: {e.Message}");
            }
        }
    }
}
