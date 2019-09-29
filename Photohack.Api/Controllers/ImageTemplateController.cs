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
        public readonly IEmotionService _emotionService;
        public readonly IPhotoService _photoService;

        public ImageTemplateController(IMusicService musicService, IEmotionService emotionService, IPhotoService photoService)
        {
            _musicService = musicService;
            _emotionService = emotionService;
            _photoService = photoService;
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
        public async Task<JsonResult> GetMusic(string phrase, string words)
        {
            if (string.IsNullOrWhiteSpace(phrase) && (words == null || words.Count() == 0))
            {
                return this.JsonApi(new MusicException(MusicErrorCode.NullArgument));
            }

            try
            {
                var tracks = await _musicService.GetMusicLinksAsync(phrase, words);

                return this.JsonApi(tracks.Data.Take(3));
            }
            catch (MusicException exception)
            {
                return this.JsonApi(exception);
            }
        }

        [HttpGet("emotion")]
        public async Task<JsonResult> GetEmotion(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return this.JsonApi(new EmotionException(EmotionErrorCode.NullArgument));
            }

            try
            {
                int emotion = await _emotionService.GetEmotionAsync(text);

                var response = new GetEmotionResponse
                {
                    Emotion = emotion
                };

                return this.JsonApi(response);
            }
            catch (EmotionException exception)
            {
                return this.JsonApi(exception);
            }
        }

        [HttpPost("photo")]
        public async Task<JsonResult> GetPhoto([FromBody]GetPhotoRequest request)
        {
            //try
            //{
            byte[] bytes = Convert.FromBase64String(request.Photo);
            var links =    await _photoService.GetFilter(request.Emotion, bytes);
            var result = new GetPhotoResponse
            {
                Links = links?.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray()
            };
            return this.JsonApi(result);
           // }
        }


    }
}
