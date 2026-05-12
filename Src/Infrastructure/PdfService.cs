using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Image = iTextSharp.text.Image;

namespace Coreapi.Infrastructure
{
    public class PdfService : IPdfService
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IConfiguration configuration;
        private readonly IS3Service s3Service;

        public PdfService(IWebHostEnvironment hostingEnvironment, IConfiguration configuration, IS3Service s3Service)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.configuration = configuration;
            this.s3Service = s3Service;
        }

        public async Task<string> SignatureToPdf(string pathPdfFile, string pathSignatureImage, string pathOutputName, int paddingTop, int paddingRight)
        {
            var webRootPath = hostingEnvironment.ContentRootPath;
            if (!File.Exists(Path.Combine(webRootPath, pathPdfFile))) return null;

            await using Stream inputPdfStream =
                new FileStream(Path.Combine(webRootPath, pathPdfFile), FileMode.Open, FileAccess.Read, FileShare.Read);
            await using Stream inputImageStream =
                new FileStream(Path.Combine(webRootPath, pathSignatureImage), FileMode.Open, FileAccess.Read, FileShare.Read);
            await using Stream outputPdfStream =
                new FileStream(Path.Combine(webRootPath, pathOutputName), FileMode.Create, FileAccess.Write, FileShare.None);

            var reader = new PdfReader(inputPdfStream);
            var stamper = new PdfStamper(reader, outputPdfStream);
            var pdfContentByte = stamper.GetOverContent(1);
            var pageSize = reader.GetPageSizeWithRotation(1);

            var image = Image.GetInstance(inputImageStream);

            image.SetAbsolutePosition(pageSize.Width - image.Width - paddingRight, pageSize.Height - image.Height - paddingTop);
            pdfContentByte.AddImage(image);
            stamper.Close();

            return "ok";

        }

        public async Task<string> SignatureToImage(string pathImageFile, string pathSignatureImage, string pathOutputName)
        {
            var webRootPath = hostingEnvironment.ContentRootPath;
            var canvas = System.Drawing.Image.FromFile(Path.Combine(webRootPath, pathImageFile));
            var gra = Graphics.FromImage(canvas);
            Bitmap mainImage = new(Path.Combine(webRootPath, pathImageFile));
            Bitmap signImage = new(Path.Combine(webRootPath, pathSignatureImage));
            gra.DrawImage(signImage, new Point(mainImage.Width - signImage.Width, signImage.Height));
            canvas.Save(Path.Combine(webRootPath, pathOutputName), System.Drawing.Imaging.ImageFormat.Jpeg);

            return "ok";
        }

        public async Task<bool> SignatureToElectProjectImage(string mapFrontPath, string backFrontPath, string signEngineerPath, string signExecutorPath, string signNezam, string signEjraii, string mapFrontPathOutput, string backFrontPathOutput, string textDes, string textEngDes, string txtMapFrontSubmit)
        {
            var sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            var webRootPath = hostingEnvironment.ContentRootPath;
            try
            {
                var mainImageMapFrontS3 = await s3Service.GetFullPath(mapFrontPath.Replace(@"\", "/"));
                var mainImageBackFrontS3 = await s3Service.GetFullPath(backFrontPath.Replace(@"\", "/"));
                var signEngineerPathS3 = await s3Service.GetFullPath(signEngineerPath.Replace(@"\", "/"));
                var signExecutorPathS3 = await s3Service.GetFullPath(signExecutorPath.Replace(@"\", "/"));



                Bitmap mainImageMapFront = new(mainImageMapFrontS3, false);
                Bitmap mainImageBackFront = new(mainImageBackFrontS3, false);

                var tempMapFront = System.Drawing.Image.FromStream(mainImageMapFrontS3);
                Bitmap canvasMapFront = new Bitmap(tempMapFront, 1350, 1820);
                var graMapFront = Graphics.FromImage(canvasMapFront);

                var tempBackFront = System.Drawing.Image.FromStream(mainImageBackFrontS3);
                Bitmap canvasBackFront = new Bitmap(tempBackFront, 1820, 1320);
                var graBackFront = Graphics.FromImage(canvasBackFront);


                Bitmap signEngineerImage = new(signEngineerPathS3);
                Bitmap resizedSignEngineerImage = new Bitmap(signEngineerImage, new Size(300, 200));

                Bitmap signExecutorImage = new(signExecutorPathS3);
                Bitmap resizedSignExecutorImage = new Bitmap(signExecutorImage, new Size(300, 200));

                Bitmap signNezamImage = new(Path.Combine(webRootPath, signNezam));
                Bitmap resizedSignNezamImage = new Bitmap(signNezamImage, new Size(200, 200));

                Bitmap signEjraiiImage = new(Path.Combine(webRootPath, signEjraii));
                Bitmap resizedSignEjraiiImage = new Bitmap(signEjraiiImage, new Size(250, 150));

                RectangleF rectf = new RectangleF(29, 200, 1200, 100);
                RectangleF rectInfoBack = new RectangleF(29, 70, 1200, 100);
                RectangleF rectfDesEng = new RectangleF(canvasMapFront.Width - 500, 1200, 450, 500);
                RectangleF rectFrontSubmit = new RectangleF(canvasMapFront.Width - 1280, 1000, 700, 500);


                graMapFront.DrawImage(resizedSignExecutorImage, new Point(canvasMapFront.Width - 310, 20));
                graMapFront.DrawImage(resizedSignEjraiiImage, new Point(canvasMapFront.Width - 780, 50));
                graMapFront.DrawImage(resizedSignEngineerImage, new Point(canvasMapFront.Width - 1300, 330));
                graMapFront.DrawImage(resizedSignEngineerImage, new Point(canvasMapFront.Width - 840, 1600));
                graMapFront.DrawImage(resizedSignNezamImage, new Point(canvasMapFront.Width - 1150, 1600));


                graMapFront.DrawString(textDes, new Font("Tahoma", 18), Brushes.Black, rectf, sf);
                graMapFront.DrawString(textEngDes, new Font("Tahoma", 18), Brushes.Black, rectfDesEng, sf);
                graMapFront.DrawString(txtMapFrontSubmit, new Font("Tahoma", 18), Brushes.Black, rectFrontSubmit, sf);

                //canvasMapFront.Save(Path.Combine(webRootPath, mapFrontPathOutput), System.Drawing.Imaging.ImageFormat.Jpeg);
                using var memoryStreamMapFront = new MemoryStream();
                canvasMapFront.Save(memoryStreamMapFront, System.Drawing.Imaging.ImageFormat.Jpeg);
                await s3Service.UploadFileAttach(memoryStreamMapFront, mapFrontPathOutput.Replace(@"\", "/"));



                graBackFront.DrawImage(resizedSignExecutorImage, new Point(canvasBackFront.Width - 650, 500));
                graBackFront.DrawImage(resizedSignEngineerImage, new Point(canvasBackFront.Width - 700, 1100));
                graBackFront.DrawImage(resizedSignNezamImage, new Point(canvasBackFront.Width - 900, 100));
                graBackFront.DrawString(textDes, new Font("Tahoma", 18), Brushes.Black, rectInfoBack);
                // graBackFront.DrawImage(resizedSignEjraiiImage, new Point(mainImageBackFront.Width - 600, 500));

                //canvasBackFront.Save(Path.Combine(webRootPath, backFrontPathOutput), System.Drawing.Imaging.ImageFormat.Jpeg);
                using var memoryStreamBackFront = new MemoryStream();
                canvasBackFront.Save(memoryStreamBackFront, System.Drawing.Imaging.ImageFormat.Jpeg);
                await s3Service.UploadFileAttach(memoryStreamBackFront, backFrontPathOutput.Replace(@"\", "/"));

            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
    }
}
