using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBlobAzure.Services
{
   
    public class AzureServiceSDK11
    {
        public CloudBlobContainer GetContainer(AzureContainer containerType)

        {

            // Obtener el contenedor de una cuenta de almacenamiento con base a la cadena de conexión de Azure

            var account = CloudStorageAccount.Parse(Settings.Constants.StorageConnection);

            var client = account.CreateCloudBlobClient();

            return client.GetContainerReference(containerType.ToString().ToLower());

        }



        public async Task<IList<string>> GetFilesListAsync(AzureContainer containerType)

        {

            // Obtiene todos los archivos blob del contenedor especificado en "containerType"

            var container = GetContainer(containerType);

            var list = new List<string>();

            BlobContinuationToken token = null;

            do

            {

                var result = await container.ListBlobsSegmentedAsync(token);

                if (result.Results.Count() > 0)

                {

                    var blobs = result.Results.Cast<CloudBlockBlob>().Select(b => b.Name);

                    list.AddRange(blobs);

                }

                token = result.ContinuationToken;

            } while (token != null);

            return list;

        }



        public async Task<byte[]> GetFileAsync(AzureContainer containerType, string name)

        {

            // Descarga el archivo blob con el nombre "name" del contenedor "containerType"

            var container = GetContainer(containerType);

            var blob = container.GetBlobReference(name);

            if (await blob.ExistsAsync())

            {

                await blob.FetchAttributesAsync();

                byte[] blobBytes = new byte[blob.Properties.Length];

                await blob.DownloadToByteArrayAsync(blobBytes, 0);

                return blobBytes;

            }

            return null;

        }



        public  async Task<string> UploadFileAsync(AzureContainer containerType, Stream stream)

        {

            // Subimos un archivo "stream" al contenedor "containerType"

            var container = GetContainer(containerType);

            await container.CreateIfNotExistsAsync();



            var name = Guid.NewGuid().ToString();

            var fileBlob = container.GetBlockBlobReference(name);

            await fileBlob.UploadFromStreamAsync(stream);



            return name;

        }



        public  async Task<bool> DeleteFileAsync(AzureContainer containerType, string name)

        {

            // Borra un archivo blob "name" de un contenedor "containerType"

            var container = GetContainer(containerType);

            var blob = container.GetBlobReference(name);

            return await blob.DeleteIfExistsAsync();

        }



        public  async Task<bool> DeleteContainerAsync(AzureContainer containerType)

        {

            // Borra un contenedor "containerType"

            var container = GetContainer(containerType);

            return await container.DeleteIfExistsAsync();

        }

    }
}
