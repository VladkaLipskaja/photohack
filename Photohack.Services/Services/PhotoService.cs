using Microsoft.AspNetCore.Hosting;
using Photohack.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Photohack.Services
{
    /// <summary>
    /// Photo service
    /// </summary>
    /// <seealso cref="Photohack.Services.IPhotoService" />
    public class PhotoService : IPhotoService
    {
        /// <summary>
        /// The client factory
        /// </summary>
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// The environment variable
        /// </summary>
        private readonly IHostingEnvironment _env;

        /// <summary>
        /// The home images URL
        /// </summary>
        private const string HomeImagesUrl = "https://crazyfacemessenger.azurewebsites.net/images/";

        /// <summary>
        /// Initializes a new instance of the <see cref="PhotoService"/> class.
        /// </summary>
        /// <param name="clientFactory">The client factory.</param>
        /// <param name="env">The env.</param>
        public PhotoService(IHttpClientFactory clientFactory, IHostingEnvironment env)
        {
            _clientFactory = clientFactory;
            _env = env;
        }

        /// <summary>
        /// Gets the filter.
        /// </summary>
        /// <param name="emotion">The emotion.</param>
        /// <param name="bytes">The bytes (image).</param>
        /// <returns>
        /// Links of photos with filters.
        /// </returns>
        public async Task<string[]> GetFilter(int emotion, byte[] bytes)
        {
            var name = SavePhoto(bytes);

            var links = await ProcessPhotoAsync(emotion, name);

            return links;
        }

        private async Task<string[]> ProcessPhotoAsync(int emotion, string name)
        {
            List<string> result = new List<string>();

            var url = HomeImagesUrl + name;

            if (PhotoEmotionTemplateConfigs.EmotionFirstTemplate.ContainsKey(emotion))
            {
                url = await GetEffectPhoto(url, PhotoEmotionTemplateConfigs.EmotionFirstTemplate[emotion]);

                result.Add(url);
            }

            var additionalFilter = url;

            if (PhotoEmotionTemplateConfigs.EmotionLastTemplate.ContainsKey(emotion))
            {
                additionalFilter = await GetEffectPhoto(additionalFilter, PhotoEmotionTemplateConfigs.EmotionLastTemplate[emotion]);

                result.Add(additionalFilter);
            }

            var frameFilter = await GetEffectPhoto(additionalFilter, PhotoEmotionTemplateConfigs.Default);

            result.Add(frameFilter);

            return result.ToArray();
        }

        private async Task<string> GetEffectPhoto(string url, int id)
        {
            var client = _clientFactory.CreateClient(PhotoApiConfigs.ClientName);

            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("image_url[1]", url));
            nvc.Add(new KeyValuePair<string, string>("template_name", id.ToString()));

            var req = new HttpRequestMessage(HttpMethod.Post, PhotoApiConfigs.RequestUri) { Content = new FormUrlEncodedContent(nvc) };
            var response = await client.SendAsync(req);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return string.Empty;
        }

        private string SavePhoto(byte[] bytes)
        {
            var webRoot = _env.WebRootPath;
            var PathWithFolderName = System.IO.Path.Combine(webRoot, "images");

            var name = Guid.NewGuid().ToString() + ".jpg";
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);

                image.Save(PathWithFolderName + "\\" + name);
            }

            return name;
        }
    }
}
