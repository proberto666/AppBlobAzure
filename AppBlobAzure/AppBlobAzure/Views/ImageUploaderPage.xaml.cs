using AppBlobAzure.Services;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppBlobAzure.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageUploaderPage : ContentPage
    {
        byte[] byteData;

        public ImageUploaderPage()
        {
            InitializeComponent();
        }

        private async void btnTakePic_Clicked(object sender, EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await App.Current.MainPage.DisplayAlert("No Camera", ":( Camara no disponible.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "AppBlobAzure",
                    Name = "BlobPicture.jpg"
                });

                if (file == null)
                    return;

                byteData = await ConvertImageFilePathToByteArray(file.Path);
                Img.Source = ImageSource.FromStream(() => new MemoryStream(byteData));

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("AppBlobAzure", $"Se generó un error ({ex.Message})", "OK");
            }

        }

        private async void btnSelectPic_Clicked(object sender, EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await App.Current.MainPage.DisplayAlert("AppBlobAzure", "No podemos acceder a tu galeria.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });

                if (file == null)
                    return;

                byteData = await ConvertImageFilePathToByteArray(file.Path);
                Img.Source = ImageSource.FromStream(() => new MemoryStream(byteData));
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("AppBlobAzure", $"Se generó un error ({ex.Message})", "OK");
            }
        }

        public async Task<byte []> ConvertImageFilePathToByteArray(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                FileStream stream = File.Open(filePath, FileMode.Open);
                byte[] bytes = new byte[stream.Length];
                await stream.ReadAsync(bytes, 0, (int)stream.Length);
                return bytes;
            }
            else
            {
                return null;
            }
        }

        private async void btnUpload_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (byteData!=null && byteData.Length>0)
                {
                    btnUpload.IsEnabled = false;
                    btnUploadSDK12.IsEnabled = false;
                    actIndicator.IsRunning = true;

                    await new AzureServiceSDK11().UploadFileAsync(AzureContainer.Image, new MemoryStream(byteData));
                }
                else
                {
                    lblMessage.Text = "Debes capturar texto para subirlo a un contenedor de azure blobs";
                    await Task.Delay(5000);
                }
            }
            catch (Exception exc)
            {
                lblMessage.Text = exc.Message;
                await Task.Delay(5000);
            }
            finally
            {
                lblMessage.Text = string.Empty;
                btnUpload.IsEnabled = true;
                btnUploadSDK12.IsEnabled = true;
                actIndicator.IsRunning = false;

            }
        }

        private async void btnUploadSDK12_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (byteData != null && byteData.Length > 0)
                {
                    btnUploadSDK12.IsEnabled = false;
                    btnUpload.IsEnabled = false;
                    actIndicator.IsRunning = true;

                    await new AzureServiceSDK12().UploadFileAsync(AzureContainer.Image, new MemoryStream(byteData));
                }
                else
                {
                    lblMessage.Text = "Debes capturar texto para subirlo a un contenedor de azure blobs";
                    await Task.Delay(5000);
                }
            }
            catch (Exception exc)
            {
                lblMessage.Text = exc.Message;
                await Task.Delay(5000);
            }
            finally
            {
                lblMessage.Text = string.Empty;
                btnUploadSDK12.IsEnabled = true;
                btnUpload.IsEnabled = true;
                actIndicator.IsRunning = false;

            }
        }
    }
}