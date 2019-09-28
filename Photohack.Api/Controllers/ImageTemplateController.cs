using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Photohack.Api.Extensions;
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

        // GET api/imagetemplate
        [HttpGet]
        public async Task<JsonResult> Get(string phrase, string words)
        {
            if (phrase == null && words == null)
            {
                throw new ArgumentNullException();
            }

            var tracks = await _musicService.GetMusicLinks(phrase, words);

            return this.JsonApi(tracks?.Data);
        }
    }
}
