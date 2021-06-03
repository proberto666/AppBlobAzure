using AppBlobAzure.Services;
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
    public partial class TextUploaderPage : ContentPage
    {
        public TextUploaderPage()
        {
            InitializeComponent();
        }

        private async void btnUpload_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(editorTxt.Text))
                {
                    btnUpload.IsEnabled = false;
                    btnUploadSDK12.IsEnabled = false;
                    actIndicator.IsRunning = true;

                    var byteData = Encoding.UTF8.GetBytes(editorTxt.Text);
                    await new AzureServiceSDK11().UploadFileAsync(AzureContainer.Text, new MemoryStream(byteData));
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

        private async void btnUploadSDK12_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(editorTxt.Text))
                {
                    btnUpload.IsEnabled = false;
                    btnUploadSDK12.IsEnabled = false;
                    actIndicator.IsRunning = true;

                    var byteData = Encoding.UTF8.GetBytes(editorTxt.Text);
                    await new AzureServiceSDK12().UploadFileAsync(AzureContainer.Text, new MemoryStream(byteData));
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
    }
}