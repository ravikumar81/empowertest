using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Winnovative;

namespace Empower.Service.Utility
{
    public static class HtmlToPdfUtility
    {
        public static byte[] GetBytePDF(StringBuilder htmlString)
        {
            // Create a HTML to PDF converter object with default settings
            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

            // Set license key received after purchase to use the converter in licensed mode
            // Leave it not set to use the converter in demo mode
            htmlToPdfConverter.LicenseKey = System.Configuration.ConfigurationManager.AppSettings["LicenseKey"];

            htmlToPdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;
            htmlToPdfConverter.PdfDocumentOptions.LiveUrlsEnabled = true;
            htmlToPdfConverter.NavigationTimeout = 500;
            htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;
            htmlToPdfConverter.PdfDocumentOptions.RightMargin = 30;
            htmlToPdfConverter.PdfDocumentOptions.LeftMargin = 30;
            htmlToPdfConverter.PdfDocumentOptions.TopMargin = 30;
            htmlToPdfConverter.PdfDocumentOptions.BottomMargin = 30;
            htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;

            //htmlToPdfConverter.PdfHeaderOptions.HeaderHeight = 20;
            //HtmlToPdfElement headerHtml = new HtmlToPdfElement("tittlestring", "rawurl");
            //headerHtml.FitHeight = true;
            //pdfConverter.PdfHeaderOptions.AddElement(headerHtml);

            //htmlToPdfConverter.PdfFooterOptions.FooterHeight = 20;
            //write the page number
            TextElement footerTextElement = new TextElement(0, 30, "Page &p; of &P;", new Font("Times New Roman", 12.0f));
            footerTextElement.TextAlign = HorizontalTextAlign.Right;
            htmlToPdfConverter.PdfFooterOptions.AddElement(footerTextElement);

            // Convert a HTML string with a base URL to a PDF document in a memory buffer
            return htmlToPdfConverter.ConvertHtml(htmlString.ToString(), "url");
        }

        public static byte[] GetBytePDF(string htmlString, string orientation = "Portrait")
        {
            // Create a HTML to PDF converter object with default settings
            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

            // Set license key received after purchase to use the converter in licensed mode
            // Leave it not set to use the converter in demo mode
            htmlToPdfConverter.LicenseKey = System.Configuration.ConfigurationManager.AppSettings["LicenseKey"];
            if (orientation == "Portrait")
            {
                htmlToPdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;
            }
            else
            {
                htmlToPdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Landscape;
            }
            htmlToPdfConverter.PdfDocumentOptions.LiveUrlsEnabled = true;
            htmlToPdfConverter.NavigationTimeout = 500;
            htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;
            htmlToPdfConverter.PdfDocumentOptions.RightMargin = 30;
            htmlToPdfConverter.PdfDocumentOptions.LeftMargin = 30;
            htmlToPdfConverter.PdfDocumentOptions.TopMargin = 30;
            htmlToPdfConverter.PdfDocumentOptions.BottomMargin = 30;
            htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;

            //htmlToPdfConverter.PdfHeaderOptions.HeaderHeight = 20;
            //HtmlToPdfElement headerHtml = new HtmlToPdfElement("tittlestring", "rawurl");
            //headerHtml.FitHeight = true;
            //pdfConverter.PdfHeaderOptions.AddElement(headerHtml);

            //htmlToPdfConverter.PdfFooterOptions.FooterHeight = 20;
            //write the page number
            TextElement footerTextElement = new TextElement(0, 30, "Page &p; of &P;", new Font("Times New Roman", 12.0f));
            footerTextElement.TextAlign = HorizontalTextAlign.Right;
            htmlToPdfConverter.PdfFooterOptions.AddElement(footerTextElement);

            // Convert a HTML string with a base URL to a PDF document in a memory buffer
            return htmlToPdfConverter.ConvertHtml(htmlString, "url");
        }

        public static byte[] GetBytePDFForPrint(StringBuilder htmlString, bool marginRequired = true)
        {
            // Create a HTML to PDF converter object with default settings
            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

            // Set license key received after purchase to use the converter in licensed mode
            // Leave it not set to use the converter in demo mode
            htmlToPdfConverter.LicenseKey = System.Configuration.ConfigurationManager.AppSettings["LicenseKey"];

            htmlToPdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;
            htmlToPdfConverter.PdfDocumentOptions.LiveUrlsEnabled = true;
            htmlToPdfConverter.NavigationTimeout = 500;

            htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;
            // htmlToPdfConverter.PdfDocumentOptions.RightMargin = 20;
            //htmlToPdfConverter.PdfDocumentOptions.LeftMargin = 20;
            htmlToPdfConverter.PdfDocumentOptions.TopMargin = 10;
            htmlToPdfConverter.PdfDocumentOptions.BottomMargin = 5;
            htmlToPdfConverter.PdfDocumentOptions.ShowHeader = false;

            //htmlToPdfConverter.PdfHeaderOptions.HeaderHeight = 20;
            //HtmlToPdfElement headerHtml = new HtmlToPdfElement("tittlestring", "rawurl");
            //headerHtml.FitHeight = true;
            //pdfConverter.PdfHeaderOptions.AddElement(headerHtml);
            //  htmlToPdfConverter.PdfDocumentOptions.AutoSizePdfPage = true;
            //  htmlToPdfConverter.PdfDocumentOptions.FitHeight = true;
            htmlToPdfConverter.PdfDocumentOptions.FitWidth = false; //false
                                                                    //  htmlToPdfConverter.PdfDocumentOptions.StretchToFit = true;

            htmlToPdfConverter.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfConverter_PrepareRenderPdfPageEvent);

            //htmlToPdfConverter.PdfFooterOptions.FooterHeight = 20;
            //write the page number
            //TextElement footerTextElement = new TextElement(0, 20, "Page &p; of &P;", new Font("Roboto", 12.0f));
            //footerTextElement.TextAlign = HorizontalTextAlign.Center;
            //htmlToPdfConverter.PdfFooterOptions.AddElement(footerTextElement);

            // Convert a HTML string with a base URL to a PDF document in a memory buffer
            return htmlToPdfConverter.ConvertHtml(htmlString.ToString(), "url");

        }

        public static string GeneratePDFAndSaveIntoBLob(string htmlString, string fileName, string uploadFolder, string azureConnection, string azureContainer, bool marginRequired = true)
        {
            // Create a HTML to PDF converter object with default settings
            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

            // Set license key received after purchase to use the converter in licensed mode
            // Leave it not set to use the converter in demo mode
            htmlToPdfConverter.LicenseKey = System.Configuration.ConfigurationManager.AppSettings["LicenseKey"];

            htmlToPdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;
            htmlToPdfConverter.PdfDocumentOptions.LiveUrlsEnabled = true;
            htmlToPdfConverter.NavigationTimeout = 500;

            htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;
            // htmlToPdfConverter.PdfDocumentOptions.RightMargin = 20;
            //htmlToPdfConverter.PdfDocumentOptions.LeftMargin = 20;
            htmlToPdfConverter.PdfDocumentOptions.TopMargin = 10;
            htmlToPdfConverter.PdfDocumentOptions.BottomMargin = 5;
            htmlToPdfConverter.PdfDocumentOptions.ShowHeader = false;

            //htmlToPdfConverter.PdfHeaderOptions.HeaderHeight = 20;
            //HtmlToPdfElement headerHtml = new HtmlToPdfElement("tittlestring", "rawurl");
            //headerHtml.FitHeight = true;
            //pdfConverter.PdfHeaderOptions.AddElement(headerHtml);
            //  htmlToPdfConverter.PdfDocumentOptions.AutoSizePdfPage = true;
            //  htmlToPdfConverter.PdfDocumentOptions.FitHeight = true;
            htmlToPdfConverter.PdfDocumentOptions.FitWidth = false; //false
                                                                    //  htmlToPdfConverter.PdfDocumentOptions.StretchToFit = true;

            htmlToPdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Best;
            htmlToPdfConverter.PdfDocumentOptions.CompressCrossReference = true;
            htmlToPdfConverter.PdfDocumentOptions.JpegCompressionEnabled = true;
            htmlToPdfConverter.PdfDocumentOptions.JpegCompressionLevel = 90;
            htmlToPdfConverter.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfConverter_PrepareRenderPdfPageEvent);

            //htmlToPdfConverter.PdfFooterOptions.FooterHeight = 20;
            //write the page number
            //TextElement footerTextElement = new TextElement(0, 20, "Page &p; of &P;", new Font("Roboto", 12.0f));
            //footerTextElement.TextAlign = HorizontalTextAlign.Center;
            //htmlToPdfConverter.PdfFooterOptions.AddElement(footerTextElement);
            using (MemoryStream stream = new MemoryStream())
            {
                htmlToPdfConverter.ConvertHtmlToStream(htmlString, "https://Empower-qa-lab1.cosdevx.com", stream);


                Log.Information("PDF Generation Completed - Time & Bytes : " + DateTime.Now + " Ticks : " + DateTime.Now.Ticks + "  Bytes: " + stream.Length);
                Log.Information("File Upload to Blob Begin - Time : " + DateTime.Now + " Ticks : " + DateTime.Now.Ticks);
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                CloudBlockBlob blockfileBlob = GetAzureBlobReference(fileName, azureConnection, azureContainer);

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                // Create or overwrite the "myblob" blob with contents from a local file.

                //using (var fileStream = File.OpenRead(filePath))
                //{
                //    blockfileBlob.UploadFromStream(stream);
                //}

                //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                //File.Delete(filePath);
                blockfileBlob.UploadFromStream(stream);
                Log.Information("File Upload to Blob End - Time : " + DateTime.Now + " Ticks : " + DateTime.Now.Ticks);
                return blockfileBlob.StorageUri.PrimaryUri.AbsoluteUri;
            }

        }

        private static CloudBlockBlob GetAzureBlobReference(string generatedFileName, string azureConnection, string azureContainer)
        {
            // Retrieve storage account from connection string.
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(azureConnection);

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(azureContainer);
            container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(generatedFileName);
            return blockBlob;
        }

        public static byte[] GetBytePDFForPrintForFall(string htmlString)
        {
            // Create a HTML to PDF converter object with default settings
            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

            // Set license key received after purchase to use the converter in licensed mode
            // Leave it not set to use the converter in demo mode
            htmlToPdfConverter.LicenseKey = System.Configuration.ConfigurationManager.AppSettings["LicenseKey"];

            htmlToPdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;
            htmlToPdfConverter.PdfDocumentOptions.LiveUrlsEnabled = true;
            htmlToPdfConverter.NavigationTimeout = 500;
            htmlToPdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.Letter;
            // htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;
            // htmlToPdfConverter.PdfDocumentOptions.RightMargin = 20;
            //htmlToPdfConverter.PdfDocumentOptions.LeftMargin = 20;
            //  htmlToPdfConverter.PdfDocumentOptions.TopMargin = 10;
            // htmlToPdfConverter.PdfDocumentOptions.BottomMargin = 5;
            //  htmlToPdfConverter.PdfDocumentOptions.ShowHeader = false;

            //htmlToPdfConverter.PdfHeaderOptions.HeaderHeight = 20;
            //HtmlToPdfElement headerHtml = new HtmlToPdfElement("tittlestring", "rawurl");
            //headerHtml.FitHeight = true;
            //pdfConverter.PdfHeaderOptions.AddElement(headerHtml);
            //  htmlToPdfConverter.PdfDocumentOptions.AutoSizePdfPage = true;
            //  htmlToPdfConverter.PdfDocumentOptions.FitHeight = true;
            //  htmlToPdfConverter.PdfDocumentOptions.FitWidth = false; //false
            //  htmlToPdfConverter.PdfDocumentOptions.StretchToFit = true;
            //if (marginRequired)
            //{
            //    htmlToPdfConverter.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfConverter_PrepareRenderPdfPageEvent);
            //}
            //htmlToPdfConverter.PdfFooterOptions.FooterHeight = 20;
            //write the page number
            //TextElement footerTextElement = new TextElement(0, 20, "Page &p; of &P;", new Font("Roboto", 12.0f));
            //footerTextElement.TextAlign = HorizontalTextAlign.Center;
            //htmlToPdfConverter.PdfFooterOptions.AddElement(footerTextElement);

            // Convert a HTML string with a base URL to a PDF document in a memory buffer
            return htmlToPdfConverter.ConvertHtml(htmlString, "url");
        }

        public static byte[] GetBytePDFForPrintForPortrait(string htmlString)
        {
            // Create a HTML to PDF converter object with default settings
            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

            // Set license key received after purchase to use the converter in licensed mode
            // Leave it not set to use the converter in demo mode
            htmlToPdfConverter.LicenseKey = System.Configuration.ConfigurationManager.AppSettings["LicenseKey"];

            htmlToPdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;
            htmlToPdfConverter.PdfDocumentOptions.LiveUrlsEnabled = true;
            htmlToPdfConverter.NavigationTimeout = 500;
            htmlToPdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.Letter;
            //Added to reduce image size on PDF
            htmlToPdfConverter.PdfDocumentOptions.JpegCompressionEnabled = true;
            htmlToPdfConverter.PdfDocumentOptions.ImagesScalingEnabled = true;

            // Convert a HTML string with a base URL to a PDF document in a memory buffer
            return htmlToPdfConverter.ConvertHtml(htmlString, "url");
        }

        public static byte[] GetBytePDFForPrintForLandscape(string htmlString)
        {
            // Create a HTML to PDF converter object with default settings
            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

            // Set license key received after purchase to use the converter in licensed mode
            // Leave it not set to use the converter in demo mode
            htmlToPdfConverter.LicenseKey = System.Configuration.ConfigurationManager.AppSettings["LicenseKey"];

            htmlToPdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Landscape;
            htmlToPdfConverter.PdfDocumentOptions.LiveUrlsEnabled = true;
            htmlToPdfConverter.NavigationTimeout = 500;
            htmlToPdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.Letter;

            return htmlToPdfConverter.ConvertHtml(htmlString, "url");
        }

        public static byte[] GetBytePDFForPrintForCollegeCombine(string htmlString)
        {
            // Create a HTML to PDF converter object with default settings
            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

            // Set license key received after purchase to use the converter in licensed mode
            // Leave it not set to use the converter in demo mode
            htmlToPdfConverter.LicenseKey = System.Configuration.ConfigurationManager.AppSettings["LicenseKey"];

            htmlToPdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;
            htmlToPdfConverter.PdfDocumentOptions.LiveUrlsEnabled = true;
            htmlToPdfConverter.NavigationTimeout = 500;
            htmlToPdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.Letter;
            //  htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;
            // htmlToPdfConverter.PdfDocumentOptions.RightMargin = 20;
            //htmlToPdfConverter.PdfDocumentOptions.LeftMargin = 20;
            //  htmlToPdfConverter.PdfDocumentOptions.TopMargin = 10;
            // htmlToPdfConverter.PdfDocumentOptions.BottomMargin = 5;
            //  htmlToPdfConverter.PdfDocumentOptions.ShowHeader = false;

            //htmlToPdfConverter.PdfHeaderOptions.HeaderHeight = 20;
            //HtmlToPdfElement headerHtml = new HtmlToPdfElement("tittlestring", "rawurl");
            //headerHtml.FitHeight = true;
            //pdfConverter.PdfHeaderOptions.AddElement(headerHtml);
            //  htmlToPdfConverter.PdfDocumentOptions.AutoSizePdfPage = true;
            //  htmlToPdfConverter.PdfDocumentOptions.FitHeight = true;
            //  htmlToPdfConverter.PdfDocumentOptions.FitWidth = false; //false
            //  htmlToPdfConverter.PdfDocumentOptions.StretchToFit = true;
            //if (marginRequired)
            //{
            //    htmlToPdfConverter.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfConverter_PrepareRenderPdfPageEvent);
            //}
            //htmlToPdfConverter.PdfFooterOptions.FooterHeight = 20;
            //write the page number
            // TextElement footerTextElement = new TextElement(0, 20, "Page &p; of &P;", new Font("Roboto", 12.0f));
            //footerTextElement.TextAlign = HorizontalTextAlign.Center;
            //htmlToPdfConverter.PdfFooterOptions.AddElement(footerTextElement);
            // HtmlToPdfVariableElement footerElement = new HtmlToPdfVariableElement("<footer> <div class='reports-footer' style=' display: flex; height: 15px;padding-left: 44px;'> <table cellpadding='0' cellspacing='0' style=' border: 1px solid #000; border-top: 0 ; '> <tr style='background-color: rgba(165, 172, 176, 0.6); -webkit-print-color-adjust: exact;'> <td style='padding-left: 9px;padding-right: 9px; width: 500px; font-family: 'Roboto Condensed !important';font-size: 8px;line-height: 11px;'>Created: 1/11/11</td><td style='padding-left: 9px; padding-right: 9px;width: 492px; text-align: right;font-family: 'Roboto Condensed !important';font-size: 8px;line-height: 11px;' class='text-right'>Page &p; of &P;</td></tr></table></div></footer>", "");

            //   htmlToPdfConverter.PdfFooterOptions.AddElement(footerElement);

            // Convert a HTML string with a base URL to a PDF document in a memory buffer
            return htmlToPdfConverter.ConvertHtml(htmlString, "url");
        }
        public static void GetBytePDFForPrintPerPage(StringBuilder htmlString, StringBuilder headerpage1, StringBuilder headerpage2, StringBuilder footer, Document pdfdocument)
        {
            // Create a HTML to PDF converter object with default settings
            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

            // Set license key received after purchase to use the converter in licensed mode
            // Leave it not set to use the converter in demo mode
            // htmlToPdfConverter.LicenseKey = System.Configuration.ConfigurationManager.AppSettings["LicenseKey"];
            pdfdocument.LicenseKey = System.Configuration.ConfigurationManager.AppSettings["LicenseKey"];
            htmlToPdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;
            htmlToPdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.Letter;
            htmlToPdfConverter.PdfDocumentOptions.LiveUrlsEnabled = true;
            htmlToPdfConverter.NavigationTimeout = 500;
            // Document pdfdocument = new Document();
            Margins margin = new Margins();
            //margin.Top = 17;
            //margin.Bottom = 18;
            //margin.Left = 36;
            //margin.Right = 36;
            // var headerTag=htmlString.ToString().Split(new string[] { "{<body>}" }, StringSplitOptions.None)[0];
            string[] page = htmlString.ToString().Split(new string[] { "{page2}" }, StringSplitOptions.None);

            PdfPage pdfPage = pdfdocument.AddPage(PdfPageSize.Letter, Margins.Empty);

            //AddHeader
            HtmlToPdfElement headerhtml = new HtmlToPdfElement(headerpage1.ToString(), "");
            pdfPage.AddHeaderTemplate(91);
            pdfPage.Header.AddElement(headerhtml);
            if (pdfPage.Index % 2 == 0)
            {
                pdfPage.Margins.Left = 18;//20
                pdfPage.Margins.Right = 36;//30
                pdfPage.Margins.Top = 17;//30
                pdfPage.Margins.Bottom = 18;//30
            }
            else
            {
                pdfPage.Margins.Left = 36;//20
                pdfPage.Margins.Right = 18;//30
                pdfPage.Margins.Top = 17;//30
                pdfPage.Margins.Bottom = 18;//30
            }
            //AddBodySection
            HtmlToPdfElement htmlToPdfElement = new HtmlToPdfElement(page[0].ToString(), "");

            pdfPage.AddElement(htmlToPdfElement);

            //AddFooter
            HtmlToPdfElement footerhtml = new HtmlToPdfElement(footer.ToString(), "");
            pdfPage.AddFooterTemplate(10);
            pdfPage.Footer.AddElement(footerhtml);

            PdfPage pdfPage2 = pdfdocument.AddPage(PdfPageSize.Letter, Margins.Empty);
            //AddHeader
            HtmlToPdfElement headerhtml2 = new HtmlToPdfElement(headerpage2.ToString(), "");
            pdfPage2.AddHeaderTemplate(64);
            pdfPage2.Header.AddElement(headerhtml2);
            if (pdfPage2.Index % 2 == 0)
            {
                pdfPage2.Margins.Left = 18;//20
                pdfPage2.Margins.Right = 36;//30
                pdfPage2.Margins.Top = 17;//30
                pdfPage2.Margins.Bottom = 18;//30
            }
            else
            {
                pdfPage2.Margins.Left = 36;//20
                pdfPage2.Margins.Right = 18;//30
                pdfPage2.Margins.Top = 17;//30
                pdfPage2.Margins.Bottom = 18;//30
            }
            //AddBodySection
            HtmlToPdfElement htmlToPdfElement2 = new HtmlToPdfElement(page[1].ToString(), "");

            pdfPage2.AddElement(htmlToPdfElement2);

            //AddFooter
            HtmlToPdfElement footerhtml2 = new HtmlToPdfElement(footer.ToString(), "");
            pdfPage2.AddFooterTemplate(10);
            pdfPage2.Footer.AddElement(footerhtml2);

            htmlToPdfElement.ConversionDelay = 2;
            if (pdfdocument.Pages.Count % 2 != 0)
            {
                //AddBlankPage
                PdfPageSize pdfPageSize = PdfPageSize.Letter;
                PdfPage blankpdfPage = pdfdocument.AddPage(pdfPageSize, Margins.Empty);

                HtmlToPdfElement bpheader = new HtmlToPdfElement(headerpage1.ToString(), "");
                blankpdfPage.AddHeaderTemplate(80);
                blankpdfPage.Header.AddElement(bpheader);
                HtmlToPdfElement bpfooter = new HtmlToPdfElement(footer.ToString(), "");
                blankpdfPage.AddFooterTemplate(10);
                blankpdfPage.Footer.AddElement(bpfooter);
            }

            //foreach (var singlepage in pdfdocument.Pages.Cast<PdfPage>())
            //{
            //    if (singlepage.Index % 2 == 0)
            //    {
            //        singlepage.Margins.Left = 18;//20
            //        singlepage.Margins.Right = 36;//30
            //        singlepage.Margins.Top = 17;//30
            //        singlepage.Margins.Bottom = 18;//30


            //        //page.ShowHeader= 15f;
            //        //page.Document.Margins.Bottom = 15f;
            //        //page.Margins.Left = 20;
            //        //page.Margins.Right = 30;
            //        //page.Margins.Top = 15;
            //        //page.Margins.Bottom = 15;
            //    }
            //    else
            //    {

            //        singlepage.Margins.Right = 18;//20
            //        singlepage.Margins.Left = 36;//30
            //        singlepage.Margins.Top = 17;//30
            //        singlepage.Margins.Bottom = 18;//30

            //        //page.Margins.Left = 30;
            //        //page.Margins.Right = 20;
            //        //page.Margins.Top = 15;
            //        //page.Margins.Bottom = 15;
            //    }
            //}

            //  byte[] outPdfBuffer;
            //  return outPdfBuffer = pdfdocument.Save();
        }

        static void htmlToPdfConverter_PrepareRenderPdfPageEvent(PrepareRenderPdfPageParams eventParams)
        {


            // Set the header visibility in first, odd and even pages
            if (eventParams.PageNumber % 2 == 0)
            {

                eventParams.Page.Margins.Left = 20;//20
                eventParams.Page.Margins.Right = 30;//30
                                                    //eventParams.Page.Margins.Left = 18;//20
                                                    // eventParams.Page.Margins.Right = 36;//30
                                                    // eventParams.Page.Margins.Top = 27;//30
                                                    // eventParams.Page.Margins.Bottom = 18;//30
            }
            else
            {
                eventParams.Page.Margins.Right = 20;
                eventParams.Page.Margins.Left = 30;
                //eventParams.Page.Margins.Right = 18;
                //  eventParams.Page.Margins.Left = 36;
                //  eventParams.Page.Margins.Top = 27;//30
                // eventParams.Page.Margins.Bottom = 18;//30
            }
        }

        public static byte[] GetBytePDFForPlayercardPrint(StringBuilder htmlString)
        {
            // Create a HTML to PDF converter object with default settings
            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

            // Set license key received after purchase to use the converter in licensed mode
            // Leave it not set to use the converter in demo mode
            htmlToPdfConverter.LicenseKey = System.Configuration.ConfigurationManager.AppSettings["LicenseKey"];

            htmlToPdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;
            htmlToPdfConverter.PdfDocumentOptions.LiveUrlsEnabled = true;
            htmlToPdfConverter.NavigationTimeout = 500;
            htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;
            // htmlToPdfConverter.PdfDocumentOptions.RightMargin = 20;
            //htmlToPdfConverter.PdfDocumentOptions.LeftMargin = 20;
            htmlToPdfConverter.PdfDocumentOptions.TopMargin = 10;
            htmlToPdfConverter.PdfDocumentOptions.BottomMargin = 5;
            htmlToPdfConverter.PdfDocumentOptions.ShowHeader = false;

            //htmlToPdfConverter.PdfHeaderOptions.HeaderHeight = 20;
            //HtmlToPdfElement headerHtml = new HtmlToPdfElement("tittlestring", "rawurl");
            //headerHtml.FitHeight = true;
            //pdfConverter.PdfHeaderOptions.AddElement(headerHtml);
            //  htmlToPdfConverter.PdfDocumentOptions.AutoSizePdfPage = true;
            //  htmlToPdfConverter.PdfDocumentOptions.FitHeight = true;
            htmlToPdfConverter.PdfDocumentOptions.FitWidth = false; //false
            //  htmlToPdfConverter.PdfDocumentOptions.StretchToFit = true;
            //htmlToPdfConverter.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfConverter_PrepareRenderPdfPageEvent);
            htmlToPdfConverter.PdfDocumentOptions.PageBreakBeforeHtmlElementsSelectors = new string[] { "page_break_before" };

            //htmlToPdfConverter.PdfFooterOptions.FooterHeight = 20;
            //write the page number
            TextElement footerTextElement = new TextElement(0, 20, "Page &p; of &P;", new Font("Roboto", 12.0f));
            footerTextElement.TextAlign = HorizontalTextAlign.Center;
            htmlToPdfConverter.PdfFooterOptions.AddElement(footerTextElement);

            // Convert a HTML string with a base URL to a PDF document in a memory buffer
            return htmlToPdfConverter.ConvertHtml(htmlString.ToString(), "url");
        }

        public static byte[] GetBytePDFForDraftBoard(string htmlString)
        {           
            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();
          
            htmlToPdfConverter.LicenseKey = System.Configuration.ConfigurationManager.AppSettings["LicenseKey"];

            htmlToPdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Landscape;
            htmlToPdfConverter.PdfDocumentOptions.LiveUrlsEnabled = true;
            htmlToPdfConverter.NavigationTimeout = 500;
            htmlToPdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.Ledger;
            //htmlToPdfConverter.PdfDocumentOptions.ShowFooter = false;
            //htmlToPdfConverter.PdfDocumentOptions.RightMargin = 2;
            //htmlToPdfConverter.PdfDocumentOptions.LeftMargin = 2;
            //htmlToPdfConverter.PdfDocumentOptions.TopMargin = 0;
            //htmlToPdfConverter.PdfDocumentOptions.BottomMargin = 2;
            //htmlToPdfConverter.PdfDocumentOptions.ShowHeader = false;

            TextElement footerTextElement = new TextElement(0, 30, "Page &p; of &P;", new Font("Times New Roman", 12.0f));
            footerTextElement.TextAlign = HorizontalTextAlign.Right;
            htmlToPdfConverter.PdfFooterOptions.AddElement(footerTextElement);
            
            return htmlToPdfConverter.ConvertHtml(htmlString, "url");
        }

    }

}
