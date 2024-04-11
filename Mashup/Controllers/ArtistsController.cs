using Mashup.Models;
using Mashup.Services;
using Microsoft.AspNetCore.Mvc;


namespace Mashup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly MusicBrainzService _musicBrainzService;
        public ArtistsController(MusicBrainzService musicBrainzService)
        {
            _musicBrainzService = musicBrainzService;
        }

        [HttpGet("{id}")]
        public async Task<Artist> GetArtist(string id)
        {
            return await _musicBrainzService.GetArtist(id);
        }

    }
}
