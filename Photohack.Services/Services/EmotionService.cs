using Microsoft.Extensions.Options;
using Photohack.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Photohack.Services
{
    public class EmotionService : IEmotionService
    {
        private readonly ParallelDotsConfiguration _configuration;

        private readonly IHttpClientFactory _clientFactory;

        public EmotionService(IHttpClientFactory clientFactory, IOptions<ParallelDotsConfiguration> configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration.Value;
        }

        public async Task<int> GetEmotionAsync(string text)
        {
            var client = _clientFactory.CreateClient(EmotionApiConfigs.ClientName);

            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("api_key", _configuration.ApiKey));
            nvc.Add(new KeyValuePair<string, string>("text", text));

            var req = new HttpRequestMessage(HttpMethod.Post, EmotionApiConfigs.RequestUri) { Content = new FormUrlEncodedContent(nvc) };
            var response = await client.SendAsync(req);

            //var response = await client.PostAsync(EmotionApiConfigs.RequestUri, new StringContent(body, Encoding.UTF8, EmotionApiConfigs.ApplicationForm));

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<EmotionResponse>();

                var k = await response.Content.ReadAsStringAsync();

                if (result?.Emotion == null)
                {
                    throw new EmotionException(EmotionErrorCode.NothingFound);
                }

                var max = result.Emotion.Angry;
                var maxEmotion = EmotionEnum.Angry;

                if (max < result.Emotion.Excited)
                {
                    max = result.Emotion.Excited;
                    maxEmotion = EmotionEnum.Excited;
                }

                if (max < result.Emotion.Fear)
                {
                    max = result.Emotion.Fear;
                    maxEmotion = EmotionEnum.Fear;
                }

                if (max < result.Emotion.Happy)
                {
                    max = result.Emotion.Happy;
                    maxEmotion = EmotionEnum.Happy;
                }

                if (max < result.Emotion.Sad)
                {
                    max = result.Emotion.Sad;
                    maxEmotion = EmotionEnum.Sad;
                }

                if (max < result.Emotion.Indifferent)
                {
                    max = result.Emotion.Indifferent;
                    maxEmotion = EmotionEnum.Indifferent;
                }

                return (int)maxEmotion;
            }

            throw new EmotionException(EmotionErrorCode.NothingFound);
        }
    }
}
