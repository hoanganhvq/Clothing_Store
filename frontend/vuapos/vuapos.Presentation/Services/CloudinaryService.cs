using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace vuapos.Presentation.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService()
        {

            //var apiKey = Environment.GetEnvironmentVariable("API_KEY");
            //var cloudName = Environment.GetEnvironmentVariable("CLOUD_NAME");
            //var apiSecret = Environment.GetEnvironmentVariable("API_SECRET");

            //Debug.Print("API key: ", apiKey);
            //Debug.Print("cloudName: ", cloudName);
            //Debug.Print("apiSecret", apiSecret);
            //if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(cloudName) || string.IsNullOrEmpty(apiSecret))
            //{
            //    Debug.Print("⚠️ Thiếu biến môi trường. Vui lòng thiết lập API_KEY, CLOUD_NAME và API_SECRET.");
            //}
            var cloudName = "dnuqb888u";
            var apiKey = "611365346874752";
            var apiSecret = "pS0SEcGZp_JXmAiPbgNB63UJHOU";

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(StorageFile file)
        {
            using var stream = await file.OpenStreamForReadAsync();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.Name, stream),
                Folder = "products"
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.ToString();
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Image
            };
            var result = await _cloudinary.DestroyAsync(deletionParams);
            if (result.Result == "ok")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}