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
    public partial class ImageBrowserPage : ContentPage
    {
        string FileNameSelected = string.Empty;

        public ImageBrowserPage()
        {
            InitializeComponent();
        }

        private async void btnGetImgFileList_Clicked(object sender, EventArgs e)
        {
            try
            {
                var fileList = await new AzureServiceSDK11().GetFilesListAsync(AzureContainer.Image);
                lstViwFiles.ItemsSource = fileList;
                DwnImg.Source = null;
                btnDelete.IsEnabled = false;
                btnDeleteSDK12.IsEnabled = false;
            }
            catch (Exception exc)
            {
                lblMessage.Text = exc.Message;
                await Task.Delay(5000);
            }
            finally
            {
                lblMessage.Text = string.Empty;
            }
        }

        private async void lstViwFiles_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem != null)
                {
                    FileNameSelected = e.SelectedItem.ToString();
                    var byteData = await new AzureServiceSDK11().GetFileAsync(AzureContainer.Image, FileNameSelected);
                    var imgSource = ImageSource.FromStream(() => new MemoryStream(byteData));

                    DwnImg.Source = imgSource;
                    btnDelete.IsEnabled = true;
                    btnDeleteSDK12.IsEnabled = true;
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
            }
        }

        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(FileNameSelected))
                {
                    if (await new AzureServiceSDK11().DeleteFileAsync(AzureContainer.Image, FileNameSelected))
                    {
                        btnGetImgFileList_Clicked(sender, e);
                    }
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
            }
        }

        private async void btnGetImgFileListSDK12_Clicked(object sender, EventArgs e)
        {
            try
            {
                var fileList = await new AzureServiceSDK12().GetFilesListAsync(AzureContainer.Image);
                lstViwFiles.ItemsSource = fileList;
                DwnImg.Source = null;
                btnDelete.IsEnabled = false;
                btnDeleteSDK12.IsEnabled = false;
            }
            catch (Exception exc)
            {
                lblMessage.Text = exc.Message;
                await Task.Delay(5000);
            }
            finally
            {
                lblMessage.Text = string.Empty;
            }
        }

        private async void btnDeleteSDK12_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(FileNameSelected))
                {
                    if (await new AzureServiceSDK12().DeleteFileAsync(AzureContainer.Image, FileNameSelected))
                    {
                        btnGetImgFileListSDK12_Clicked(sender, e);
                    }
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
            }
        }
    }
}