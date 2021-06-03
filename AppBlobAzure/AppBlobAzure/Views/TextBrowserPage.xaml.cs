using AppBlobAzure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppBlobAzure.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TextBrowserPage : ContentPage
    {
        string FileNameSelected = string.Empty;

        public TextBrowserPage()
        {
            InitializeComponent();
        }

        private async void btnGetTxtFileList_Clicked(object sender, EventArgs e)
        {
            try
            {
                var fileList = await new AzureServiceSDK11().GetFilesListAsync(AzureContainer.Text);
                lstViwFiles.ItemsSource = fileList;
                editorTxt.Text = string.Empty;
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
                if (e.SelectedItem!=null)
                {
                    FileNameSelected = e.SelectedItem.ToString();
                    var byteData = await new AzureServiceSDK11().GetFileAsync(AzureContainer.Text, FileNameSelected);
                    var text = Encoding.UTF8.GetString(byteData);
                    editorTxt.Text = text;
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
                    if(await new AzureServiceSDK11().DeleteFileAsync(AzureContainer.Text, FileNameSelected)){
                        btnGetTxtFileList_Clicked(sender, e);
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

        private async void btnGetTxtFileListSDK12_Clicked(object sender, EventArgs e)
        {

            try
            {
                var fileList = await new AzureServiceSDK12().GetFilesListAsync(AzureContainer.Text);
                lstViwFiles.ItemsSource = fileList;
                editorTxt.Text = string.Empty;
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
                    if (await new AzureServiceSDK12().DeleteFileAsync(AzureContainer.Text, FileNameSelected))
                    {
                        btnGetTxtFileListSDK12_Clicked(sender, e);
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