using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Pdf;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PDFReader
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            PdfPages = new ObservableCollection<BitmapImage>();
            this.Loaded += MainPage_Loaded;
        }

        PdfDocument pdfDocument;
        public ObservableCollection<BitmapImage> PdfPages;
        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".pdf");
            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                try
                {

                    pdfDocument = await PdfDocument.LoadFromFileAsync(file);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            if (pdfDocument != null)
            {
                uint pageCount = pdfDocument.PageCount;
                for (uint pageIndex = 0; pageIndex < pageCount; pageIndex++)
                {
                    using (PdfPage page = pdfDocument.GetPage(pageIndex))
                    {
                        var stream = new InMemoryRandomAccessStream();
                        await page.RenderToStreamAsync(stream);
                        BitmapImage src = new BitmapImage();
                        await src.SetSourceAsync(stream);
                        PdfPages.Add(src);
                    }
                }
            }
        }
    }
}
