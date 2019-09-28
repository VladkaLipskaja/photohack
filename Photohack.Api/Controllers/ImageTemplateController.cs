using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Photohack.Api.Extensions;
using Photohack.Models;
using Photohack.Services;

namespace Photohack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageTemplateController : ControllerBase
    {
        public readonly IMusicService _musicService;

        public ImageTemplateController(IMusicService musicService)
        {
            _musicService = musicService;
        }

        /// <summary>
        /// Gets tracks (depends on the whole phrase and main words).
        /// </summary>
        /// <param name="phrase">The whole phrase.</param>
        /// <param name="words">The words, separated with spaces.</param>
        /// <returns>List of tracks.</returns>
        /// <exception cref="ArgumentNullException">
        /// If phrase and words are null.
        /// </exception>
        [HttpGet]
        public async Task<JsonResult> Get(string phrase, string words)
        {
            if (string.IsNullOrWhiteSpace(phrase) && (words == null || words.Count() == 0))
            {
                return this.JsonApi(new MusicException(MusicErrorCode.NullArgument));
            }

            try
            {

                var tracks = await _musicService.GetMusicLinks(phrase, words);

                return this.JsonApi(tracks.Data.Take(3));
            }
            catch (MusicException exception)
            {
                return this.JsonApi(exception);
            }
        }
    }
}
