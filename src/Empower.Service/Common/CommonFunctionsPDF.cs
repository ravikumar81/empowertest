using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using PdfSharp.Pdf.IO;
using Serilog;
using Empower.Service.Utility;
using System.Threading;

namespace Empower.Service.Common
{
    static class CommonFunctionsPDF
    {

        //public static void PDFMerger(string fileName, IDocumentUploadService documentUploadService, string playerStorageFolder, string azureConnection, string htmlPrintReportsStorageContainer)
        //{
        //    //IEnumerable<byte[]> stickers;
        //    string filenames = null;
        //    try
        //    {
        //        using (PdfDocument outPdf = new PdfDocument())
        //        {
        //            foreach (string file in Directory.EnumerateFiles(ConfigurationManager.AppSettings["PDFFilePath"] + "\\" + fileName + "\\", "*.pdf").OrderBy(filename => Int32.Parse(filename.Split('.')[0].Split('_')[filename.Split('.')[0].Split('_').Length - 1])).ToList())
        //            {
        //                using (PdfDocument from = PdfReader.Open(file, PdfDocumentOpenMode.Import))
        //                {
        //                    filenames = file;
        //                    for (int i = 0; i < from.PageCount; i++)
        //                    {
        //                        outPdf.AddPage(from.Pages[i]);
        //                    }
        //                }
        //            }
        //            outPdf.Save(ConfigurationManager.AppSettings["PDFFilePath"] + "\\" + fileName + "\\" + "fullPDF.pdf");

        //            GC.Collect();
        //            GC.WaitForPendingFinalizers();
        //            documentUploadService.UploadReport(File.ReadAllBytes(ConfigurationManager.AppSettings["PDFFilePath"] + fileName + "\\" + "fullPDF.pdf"), fileName + ".pdf", playerStorageFolder, azureConnection, htmlPrintReportsStorageContainer);

        //            if (Directory.Exists(ConfigurationManager.AppSettings["PDFFilePath"] + "\\" + fileName))
        //            {
        //                GC.Collect();
        //                GC.WaitForPendingFinalizers();
        //                Directory.Delete(ConfigurationManager.AppSettings["PDFFilePath"] + "\\" + fileName, true);
        //            }
        //            Log.Information("All PDF's Merged");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("PDF Merge Exception - " + ex.Message + "FileName - " + filenames);
        //        throw ex;
        //    }
        //}


        public static void GeneratePDF(string reporthtml, string filename, object lockVar,bool isLandScape = false)
        {
            try
            {
                // Log.Information("Thread Id " + Thread.CurrentThread.ManagedThreadId + " PDF Generation Started - Time : " + DateTime.Now + " Ticks : " + DateTime.Now.Ticks);
                //Thread.Sleep(120000);
                // string pdf = HtmlToPdfUtility.GeneratePDFAndSaveIntoBLob(fallreporthtml, filename, _playerStorageFolder, _azureConnection, _htmlPrintReportsStorageContainer, false);
                byte[] pdf = null;
                try
                {
                    if (isLandScape)
                        pdf = HtmlToPdfUtility.GetBytePDFForPrintForLandscape(reporthtml);
                    else
                        pdf = HtmlToPdfUtility.GetBytePDFForPrintForFall(reporthtml);
                }
                catch (Exception ex)
                {
                    Log.Error("Thread Exception PDF generation : " + ex.Message + " " + ex.StackTrace + " - " + DateTime.Now + " Ticks : " + DateTime.Now.Ticks);
                    throw ex;
                }
                // Log.Information("File Upload to Blob Begin - Time : " + DateTime.Now + " Ticks : " + DateTime.Now.Ticks);
                //  string fileName = _documentUploadService.UploadReport(pdf, filename, _playerStorageFolder, _azureConnection, _htmlPrintReportsStorageContainer);
                // Log.Information("File Upload to Blob End - Time : " + DateTime.Now + " Ticks : " + DateTime.Now.Ticks);
                // blobUrl = fileName;
                // Log.Information("Thread Id " + Thread.CurrentThread.ManagedThreadId + " " + DateTime.Now + " Http URL for the Blob - " + blobUrl);
                lock (lockVar)
                {
                    if (!Directory.Exists(ConfigurationManager.AppSettings["PDFFilePath"] + "\\" + filename))
                    {
                        Directory.CreateDirectory(ConfigurationManager.AppSettings["PDFFilePath"] + "\\" + filename);
                    }
                    using (FileStream fs = new FileStream(ConfigurationManager.AppSettings["PDFFilePath"] + "\\" + filename + "\\" + Thread.CurrentThread.Name + ".pdf", FileMode.OpenOrCreate))
                    {
                        fs.Write(pdf, 0, pdf.Length);
                    }

                }
            }
            finally { }
        }
    }
}
