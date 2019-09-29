using Photohack.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Photohack.Services
{
    public class MusicService : IMusicService
    {
        private readonly IHttpClientFactory _clientFactory;

        public MusicService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<MusicTrack> GetMusicLinksAsync(string phrase, string words)
        {
            MusicTrack result = null;

            while (phrase?.Length > 0 && (result?.Data == null || result.Data.Count == 0))
            {
                result = await GetTracks(phrase);

                phrase = phrase.Substring(0, Math.Max(0, phrase.LastIndexOf(' ')));
            }

            if (result?.Data != null && result.Data.Count > 0)
            {
                return result;
            }

            if (words?.Length > 0)
            {
                foreach (var word in words.Split(' '))
                {
                    result = await GetTracks(word);

                    if (result?.Data != null && result.Data.Count > 0)
                    {
                        return result;
                    }
                }
            }

            throw new MusicException(MusicErrorCode.NothingFound);
        }

        private async Task<MusicTrack> GetTracks(string phrase)
        {
            var client = _clientFactory.CreateClient(MusicApiConfigs.ClientName);

            var response = await client.GetAsync(MusicApiConfigs.RequestUri + phrase);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<MusicTrack>();
            }
            else
            {
                return null;
            }
        }
    }
}
