using Mashup.Controllers;
using Mashup.Models;
using Mashup.Services;
using Xunit;

namespace MashupAPI.Test
{
    public class ArtistsControllerTest
    {
        readonly ArtistsController _artistController;
        readonly MusicBrainzService _musicBrainzService;

        public ArtistsControllerTest()
        {
            var httpClient = new HttpClient();
            var wikipediaService = new WikipediaService(httpClient);
            var coverArtService = new CoverArtService(httpClient);
            var wikidataService = new WikidataService(httpClient, wikipediaService);
            _musicBrainzService = new MusicBrainzService(httpClient, wikidataService, wikipediaService, coverArtService);
            _artistController = new ArtistsController(_musicBrainzService);
        }

        [Theory]
        [InlineData("5b11f4ce-a62d-471e-81fc-a69a8278c7da")]
        public async Task GetArtistTest(string id)
        {
            //Arrange
            string validId = id;

            //Act
            var artist = await _artistController.GetArtist(validId);

            //Assert
            Assert.IsType<Artist>(artist);

        }

    }
}