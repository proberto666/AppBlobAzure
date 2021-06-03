using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AppBlobAzure.Services
{
    public enum AzureContainer
    {
        Image,
        Text
    }

    public class AzureServiceSDK12
    {
        public async Task<BlobContainerClient> GetContainerAsync(AzureContainer type)
        {
            BlobServiceClient serviceClient = new BlobServiceClient(Settings.Constants.StorageConnection);
            BlobContainerClient containerClient = serviceClient.GetBlobContainerClient(Enum.GetName(typeof(AzureContainer),type).ToLower());
            await containerClient.CreateIfNotExistsAsync();
            return containerClient;
        }

        public async Task<IList<string>> GetFilesListAsync(AzureContainer type)
        {
            BlobContainerClient container = await GetContainerAsync(type);
            var list = new List<string>();
            await foreach(BlobItem bloblItem in container.GetBlobsAsync())
            {
                list.Add(bloblItem.Name);
            }
            return list;
        }

        public async Task<byte[]> GetFileAsync(AzureContainer containerType, string name)
        {
            // Descarga el archivo blob con el nombre "name" del contenedor "containerType"
            string localPath = "./temp/";
            string fileName = $"{name}.tmp";
            string filePath = Path.Combine(localPath, fileName);

            BlobContainerClient container = await GetContainerAsync(containerType);
            BlobClient blob = container.GetBlobClient(name);
            if(await blob.ExistsAsync())
            {
                await blob.DownloadToAsync(filePath);
                FileStream stream = File.Open(filePath, FileMode.Open);
                byte[] blobBytes = new byte[stream.Length];
                await stream.ReadAsync(blobBytes, 0, (int)stream.Length);
                return blobBytes;
            }
            return null;

        }

        public async Task<string> UploadFileAsync(AzureContainer containerType, Stream stream)
        {
            // Subimos un archivo "stream" al contenedor "containerType"
            var name = Guid.NewGuid().ToString();
            BlobContainerClient container = await GetContainerAsync(containerType);
            BlobClient blob = container.GetBlobClient(name);
            await blob.UploadAsync(stream, true);
            return name;
        }

        public async Task<bool> DeleteFileAsync(AzureContainer containerType, string name)
        {
            // Borra un archivo blob "name" de un contenedor "containerType"
            BlobContainerClient container = await GetContainerAsync(containerType);
            BlobClient blob = container.GetBlobClient(name);
            return await blob.DeleteIfExistsAsync();
        }

        public async Task<bool> DeleteContainerAsync(AzureContainer containerType)
        {
            // Borra un contenedor "containerType"
            BlobContainerClient container = await GetContainerAsync(containerType);
            return await container.DeleteIfExistsAsync();
        }
    }
}
