using Microsoft.AspNetCore.Hosting;
using Photohack.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Photohack.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHostingEnvironment _env;

        public PhotoService(IHttpClientFactory clientFactory, IHostingEnvironment env)
        {
            _clientFactory = clientFactory;
            _env = env;
        }

        public Task<string> GetFilter(int filter, string url)
        {
            throw new NotImplementedException();
        }

        public async Task<string[]> ProcessPhoto(int emotion)
        {
            List<string> result = new List<string>();

            var url = "https://i.gyazo.com/c1d535f144c094db5806c10c03f6c118.png";

            var firstStep = url;

            if (PhotoEmotionTemplateConfigs.EmotionFirstTemplate.ContainsKey(emotion))
            {
                firstStep = await GetEffectPhoto(firstStep, PhotoEmotionTemplateConfigs.EmotionFirstTemplate[emotion]);

                result.Add(firstStep);
            }

            var secondStep = firstStep;

            if (PhotoEmotionTemplateConfigs.EmotionLastTemplate.ContainsKey(emotion))
            {
                secondStep = await GetEffectPhoto(secondStep, PhotoEmotionTemplateConfigs.EmotionLastTemplate[emotion]);

                result.Add(secondStep);
            }

            var defaultUrl = await GetEffectPhoto(secondStep, PhotoEmotionTemplateConfigs.Default);

            result.Add(defaultUrl);

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

        public Task SavePhoto(byte[] bytes, string name)
        {
            var webRoot = _env.WebRootPath;
            var PathWithFolderName = System.IO.Path.Combine(webRoot, "images");

            var webClient = new WebClient();
            bytes = webClient.DownloadData("https://i.gyazo.com/c1d535f144c094db5806c10c03f6c118.png");

            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }

            image.Save(PathWithFolderName + "/ImageName.png");


            // Try to create the directory.
            //DirectoryInfo di = Directory.CreateDirectory(PathWithFolderName + "\\" + Guid.NewGuid() + ".jpg");


            //string Base64String = eventMaster.BannerImage.Replace("data:image/png;base64,", "");

            //File.WriteAllBytes(PathWithFolderName, image);


            return Task.CompletedTask;
        }
    }
}
