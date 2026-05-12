using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Stimulsoft.Base;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Export;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CheckListForm = Coreapi.Common.ViewModels.CheckListForm;
using CommentEngForm = Coreapi.Common.ViewModels.CommentEngForm;


namespace Coreapi.Infrastructure;

public class ReportService:IReportService
{

    private readonly IWebHostEnvironment hostingEnvironment;
    private readonly IConfiguration configuration;
    private readonly IS3Service s3Service;

    public ReportService(IWebHostEnvironment hostingEnvironment, IConfiguration configuration, IS3Service s3Service)
    {
        Stimulsoft.Base.StiLicense.Key =
            "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHn0s4gy0Fr5YoUZ9V00Y0igCSFQzwEqYBh/N77k4f0fWXTHW5rqeBNLkaurJDenJ9o97TyqHs9HfvINK18Uwzsc/bG01Rq+x3H3Rf+g7AY92gvWmp7VA2Uxa30Q97f61siWz2dE5kdBVcCnSFzC6awE74JzDcJMj8OuxplqB1CYcpoPcOjKy1PiATlC3UsBaLEXsok1xxtRMQ283r282tkh8XQitsxtTczAJBxijuJNfziYhci2jResWXK51ygOOEbVAxmpflujkJ8oEVHkOA/CjX6bGx05pNZ6oSIu9H8deF94MyqIwcdeirCe60GbIQByQtLimfxbIZnO35X3fs/94av0ODfELqrQEpLrpU6FNeHttvlMc5UVrT4K+8lPbqR8Hq0PFWmFrbVIYSi7tAVFMMe2D1C59NWyLu3AkrD3No7YhLVh7LV0Tttr/8FrcZ8xirBPcMZCIGrRIesrHxOsZH2V8t/t0GXCnLLAWX+TNvdNXkB8cF2y9ZXf1enI064yE5dwMs2fQ0yOUG/xornE";


        this.hostingEnvironment = hostingEnvironment;
        this.configuration = configuration;
        this.s3Service = s3Service;

		var fontPath = Path.Combine(hostingEnvironment.ContentRootPath, "fonts");

		if (Directory.Exists(fontPath))
		{
			var fontFiles = Directory.GetFiles(fontPath, "*.ttf", SearchOption.AllDirectories);

			foreach (var font in fontFiles)
			{
				StiFontCollection.AddFontFile(font);
			}
		}
		Console.WriteLine($"Loaded fonts: {StiFontCollection.GetFontFamilies().Count}");
	}


    public async Task<string> GetApprovedCommentForm(byte[] assign, IEnumerable<CommentEngForm> commentEngForms, ElectProject electProject,Engineer engineer)
    {
 
        var webRootPath = hostingEnvironment.ContentRootPath;

        var mrtPath = Path.Combine(webRootPath,"Upload", "Reports", "ApprovedCommentEngForm.mrt");

            var report = new StiReport();
            var pdfSettings = new StiPdfExportSettings
            {
                EmbeddedFonts = true
            };

            // loaded report
            report.Load(mrtPath);
            report.Dictionary.Databases.Clear();
            var data = new DataSet("Demo");
            var dataTable = Helper.LinqToDataTable(commentEngForms);
            data.Tables.Add(dataTable);
            report.Dictionary.Databases.Clear();

            report.Dictionary.Variables["solarDate"].Value = Helper.MiladiToShamsi(DateTime.Now.Date);
            report.Dictionary.Variables["landlordName"].Value = electProject.LandlordName;
            report.Dictionary.Variables["fileNumber"].Value = electProject.FileNumber.ToString();
            report.Dictionary.Variables["electRequestNumber"].Value = electProject.ElectRequestNumber.ToString();
            report.Dictionary.Variables["address"].Value = electProject.Address;
            report.Dictionary.Variables["engName"].Value = engineer.FullName;

            if (!report.Dictionary.Resources.Contains("engAssign"))
            report.Dictionary.Resources.Add(new StiResource("engAssign", StiResourceType.Image, assign));
            else
            report.Dictionary.Resources["engAssign"].Content = assign;
        // -------------------------

        // Register data to report and render
            report.RegData("Demo","Demo", dataTable);
            if (report.Dictionary.DataSources.Count == 0)
            {
                throw new NotFoundException("خطا در ایجاد گزارش");
            }

            await report.RenderAsync();
        //--------------------
        
        // Pdf
            using var pdfStream = new MemoryStream();
            await report.ExportDocumentAsync(StiExportFormat.Pdf, pdfStream, pdfSettings);
            pdfStream.Position = 0;

        await s3Service.UploadFileAttach(pdfStream, $"Upload/ElectProjects/{electProject.Id}/approved-comment-form.pdf");
        //-----------------------


        return "ok";
    }

    public async Task<string> GetApprovedCommentFormBigProject(List<(Engineer engineer, byte[] assignImage)> engineerAssignPairs, IEnumerable<CommentEngForm> commentEngForms,
        ElectProject electProject)
    {
        var webRootPath = hostingEnvironment.ContentRootPath;

        var mrtPath = Path.Combine(webRootPath, "Upload", "Reports", "ApprovedCommentEngFormBigProject.mrt");

        var report = new StiReport();
        var pdfSettings = new StiPdfExportSettings { EmbeddedFonts = true };

        report.Load(mrtPath);
        report.Dictionary.Databases.Clear();
        report.Dictionary.Resources.Clear();

        // Prepare data
        var data = new DataSet("Demo");
        var dataTable = Helper.LinqToDataTable(commentEngForms);
        data.Tables.Add(dataTable);
        report.RegData("Demo", "Demo", dataTable);

        if (report.Dictionary.DataSources.Count == 0)
            throw new NotFoundException("خطا در ایجاد گزارش");

        // Set project and general variables
        report.Dictionary.Variables["solarDate"].Value = Helper.MiladiToShamsi(DateTime.Now.Date);
        report.Dictionary.Variables["landlordName"].Value = electProject.LandlordName;
        report.Dictionary.Variables["fileNumber"].Value = electProject.FileNumber.ToString();
        report.Dictionary.Variables["electRequestNumber"].Value = electProject.ElectRequestNumber.ToString();
        report.Dictionary.Variables["address"].Value = electProject.Address;

        // Combine engineer full names
        var engineerNames = string.Join(" - ", engineerAssignPairs.Select(e => e.engineer.FullName));
        report.Dictionary.Variables["engName"].Value = engineerNames;

        // Add multiple assign images to resources (engAssign0, engAssign1, etc.)
        for (int i = 0; i < engineerAssignPairs.Count; i++)
        {
            var imageName = $"engAssign{i}";
            var assignImage = engineerAssignPairs[i].assignImage;

            if (!report.Dictionary.Resources.Contains(imageName))
                report.Dictionary.Resources.Add(new StiResource(imageName, StiResourceType.Image, assignImage));
            else
                report.Dictionary.Resources[imageName].Content = assignImage;
        }

        await report.RenderAsync();

        // Export to PDF
        using var pdfStream = new MemoryStream();
        await report.ExportDocumentAsync(StiExportFormat.Pdf, pdfStream, pdfSettings);
        pdfStream.Position = 0;

        var filePath = $"Upload/ElectProjects/{electProject.Id}/approved-comment-form.pdf";
        await s3Service.UploadFileAttach(pdfStream, filePath);

        return "ok";
    }


    public async Task<string> GetApprovedCheckListForm(byte[] assign, IEnumerable<CheckListForm> checkListEngForms, ElectProject electProject, Engineer engineer, long workPermitNum,string solarDeliverEng)
    {

        var webRootPath = hostingEnvironment.ContentRootPath;
        var mrtPath = Path.Combine(webRootPath, "Upload", "Reports", "ApprovedCheckListEngForm.mrt");

        var report = new StiReport();
        var pdfSettings = new StiPdfExportSettings
        {
            EmbeddedFonts = true
        };

        // loaded report
        report.Load(mrtPath);
        report.Dictionary.Databases.Clear();
        var data = new DataSet("Demo");
        var dataTable = Helper.LinqToDataTable(checkListEngForms);
        data.Tables.Add(dataTable);
        report.Dictionary.Databases.Clear();

        report.Dictionary.Variables["engName"].Value = engineer.FullName;
        report.Dictionary.Variables["solarDate"].Value = Helper.MiladiToShamsi(DateTime.Now.Date);
        report.Dictionary.Variables["landlordName"].Value = electProject.LandlordName;
        report.Dictionary.Variables["address"].Value = electProject.Address; 
        report.Dictionary.Variables["fileNumber"].Value = electProject.FileNumber.ToString();
        report.Dictionary.Variables["licenseNumber"].Value = electProject.LicenseNumber.ToString();
        report.Dictionary.Variables["numberOfFloor"].Value = electProject.NumberOfFloor.ToString();
        report.Dictionary.Variables["workPermitNum"].Value = workPermitNum.ToString();
        report.Dictionary.Variables["solarDeliverEng"].Value = solarDeliverEng;
        
        
        // Add Eng Assign to report
        if (!report.Dictionary.Resources.Contains("engAssign"))
            report.Dictionary.Resources.Add(new StiResource("engAssign", StiResourceType.Image, assign));
        else
            report.Dictionary.Resources["engAssign"].Content = assign;
        // -------------------------


        // Register data to report and render
        report.RegData("Demo", "Demo", dataTable);
        if (report.Dictionary.DataSources.Count == 0)
        {
            throw new NotFoundException("خطا در ایجاد گزارش");
        }

        await report.RenderAsync();
        //--------------------

        // Pdf
        using var pdfStream = new MemoryStream();
        await report.ExportDocumentAsync(StiExportFormat.Pdf, pdfStream, pdfSettings);
        pdfStream.Position = 0;
        await s3Service.UploadFileAttach(pdfStream, $"Upload/ElectProjects/{electProject.Id}/approved-check-list-form.pdf");
        //-----------------------

        return "ok";
    }

    public async Task<string> GetApprovedCheckListFormBigProject(
        List<(Engineer engineer, byte[] assignImage)> engineerAssignPairs,
        IEnumerable<CheckListForm> checkListEngForms, 
        ElectProject electProject, 
        string workPermitNum)
    {

        var webRootPath = hostingEnvironment.ContentRootPath;
        var mrtPath = Path.Combine(webRootPath, "Upload", "Reports", "ApprovedCheckListEngFormBigProject.mrt");

        var report = new StiReport();
        var pdfSettings = new StiPdfExportSettings
        {
            EmbeddedFonts = true
        };

        // loaded report
        report.Load(mrtPath);
        report.Dictionary.Databases.Clear();
        var data = new DataSet("Demo");
        var dataTable = Helper.LinqToDataTable(checkListEngForms);
        data.Tables.Add(dataTable);
        report.Dictionary.Databases.Clear();

        // Combine engineer full names
        var engineerNames = string.Join(" - ", engineerAssignPairs.Select(e => e.engineer.FullName));
        report.Dictionary.Variables["engName"].Value = engineerNames;
        report.Dictionary.Variables["solarDate"].Value = Helper.MiladiToShamsi(DateTime.Now.Date);
        report.Dictionary.Variables["landlordName"].Value = electProject.LandlordName;
        report.Dictionary.Variables["address"].Value = electProject.Address;
        report.Dictionary.Variables["fileNumber"].Value = electProject.FileNumber.ToString();
        report.Dictionary.Variables["licenseNumber"].Value = electProject.LicenseNumber;
        report.Dictionary.Variables["numberOfFloor"].Value = electProject.NumberOfFloor.ToString();
        report.Dictionary.Variables["workPermitNum"].Value = workPermitNum;


        // Add multiple assign images to resources (engAssign0, engAssign1, etc.)
        for (int i = 0; i < engineerAssignPairs.Count; i++)
        {
            var imageName = $"engAssign{i}";
            var assignImage = engineerAssignPairs[i].assignImage;

            if (!report.Dictionary.Resources.Contains(imageName))
                report.Dictionary.Resources.Add(new StiResource(imageName, StiResourceType.Image, assignImage));
            else
                report.Dictionary.Resources[imageName].Content = assignImage;
        }


        // Register data to report and render
        report.RegData("Demo", "Demo", dataTable);
        if (report.Dictionary.DataSources.Count == 0)
        {
            throw new NotFoundException("خطا در ایجاد گزارش");
        }

        await report.RenderAsync();
        //--------------------

        // Pdf
        using var pdfStream = new MemoryStream();
        await report.ExportDocumentAsync(StiExportFormat.Pdf, pdfStream, pdfSettings);
        pdfStream.Position = 0;
        await s3Service.UploadFileAttach(pdfStream, $"Upload/ElectProjects/{electProject.Id}/approved-check-list-form.pdf");
        //-----------------------


        return "ok";
    }


    public async Task<string> GetApprovedErtForm(byte[] assign, byte[] border,
        ElectProjectErtForm electProjectErtForm, Engineer engineer, string solarDeliverEng)
    {

        var webRootPath = hostingEnvironment.ContentRootPath;
        var mrtPath = Path.Combine(webRootPath, "Upload", "Reports", "ApprovedErtForm.mrt");

        var report = new StiReport();
        var pdfSettings = new StiPdfExportSettings
        {
            EmbeddedFonts = true
        };

        // loaded report
        report.Load(mrtPath);
        report.Dictionary.Databases.Clear();
        var data = new DataSet("Demo");
        var dataTable = Helper.LinqToDataTable(electProjectErtForm);
        data.Tables.Add(dataTable);
        report.Dictionary.Databases.Clear();

        report.Dictionary.Variables["engName"].Value = engineer.FullName;
        report.Dictionary.Variables["solarDate"].Value = Helper.MiladiToShamsi(DateTime.Now.Date);
        report.Dictionary.Variables["solarDeliverEng"].Value = solarDeliverEng;
        report.Dictionary.Variables["landlordName"].Value = electProjectErtForm.ElectProject.LandlordName;
        report.Dictionary.Variables["address"].Value = electProjectErtForm.ElectProject.Address;
        report.Dictionary.Variables["fileNumber"].Value = electProjectErtForm.ElectProject.FileNumber.ToString();


        // Add Eng Assign to report
        if (!report.Dictionary.Resources.Contains("engAssign"))
            report.Dictionary.Resources.Add(new StiResource("engAssign", StiResourceType.Image, assign));
        else
            report.Dictionary.Resources["engAssign"].Content = assign;

        // Add border to report
        if (!report.Dictionary.Resources.Contains("border"))
            report.Dictionary.Resources.Add(new StiResource("border", StiResourceType.Image, border));
        else
            report.Dictionary.Resources["border"].Content = border;


        // Register data to report and render
        report.RegData("Demo", "Demo", dataTable);
        if (report.Dictionary.DataSources.Count == 0)
        {
            throw new NotFoundException("خطا در ایجاد گزارش");
        }

        await report.RenderAsync();
        //--------------------

        // Pdf
        using var pdfStream = new MemoryStream();
        await report.ExportDocumentAsync(StiExportFormat.Pdf, pdfStream, pdfSettings);
        pdfStream.Position = 0;
        await s3Service.UploadFileAttach(pdfStream, $"Upload/ElectProjects/{electProjectErtForm.ElectProject.Id}/approved-ert-form.pdf");
        //-----------------------

        return "ok";
    }

    public async Task<string> GetApprovedErtForm(byte[] assign,
     ElectProjectErtForm electProjectErtForm, Engineer engineer, string solarDeliverEng, byte[] crookyOfElectrode, long workPermitNum)
    {

        var webRootPath = hostingEnvironment.ContentRootPath;
        var mrtPath = Path.Combine(webRootPath, "Upload", "Reports", "ApprovedErtFormNew.mrt");

        var report = new StiReport();
        var pdfSettings = new StiPdfExportSettings
        {
            EmbeddedFonts = true
        };

        // loaded report
        report.Load(mrtPath);
        report.Dictionary.Databases.Clear();
        var data = new DataSet("Demo");
        var dataTable = Helper.LinqToDataTable(electProjectErtForm);
        data.Tables.Add(dataTable);
        report.Dictionary.Databases.Clear();

        report.Dictionary.Variables["engName"].Value = engineer.FullName;
        report.Dictionary.Variables["workPermitNum"].Value = workPermitNum.ToString();
        report.Dictionary.Variables["solarDate"].Value = Helper.MiladiToShamsi(DateTime.Now.Date);
        report.Dictionary.Variables["solarDeliverEng"].Value = solarDeliverEng;
        report.Dictionary.Variables["landlordName"].Value = electProjectErtForm.ElectProject.LandlordName;
        report.Dictionary.Variables["address"].Value = electProjectErtForm.ElectProject.Address;
        report.Dictionary.Variables["fileNumber"].Value = electProjectErtForm.ElectProject.FileNumber.ToString();
        report.Dictionary.Variables["buildingType"].Value = electProjectErtForm.ElectProject.BuildingTypeEnum.GetDisplayName();


        // Add Eng Assign to report
        if (!report.Dictionary.Resources.Contains("engAssign"))
            report.Dictionary.Resources.Add(new StiResource("engAssign", StiResourceType.Image, assign));
        else
            report.Dictionary.Resources["engAssign"].Content = assign;
        // -------------------------

        // Add Crooky Of Electrode to report
        if (!report.Dictionary.Resources.Contains("crooky"))
            report.Dictionary.Resources.Add(new StiResource("crooky", StiResourceType.Image, crookyOfElectrode));
        else
            report.Dictionary.Resources["crooky"].Content = assign;
        // -------------------------

        // Register data to report and render
        report.RegData("Demo", "Demo", dataTable);
        if (report.Dictionary.DataSources.Count == 0)
        {
            throw new NotFoundException("خطا در ایجاد گزارش");
        }

        await report.RenderAsync();
        //--------------------

        // Pdf
        using var pdfStream = new MemoryStream();
        await report.ExportDocumentAsync(StiExportFormat.Pdf, pdfStream, pdfSettings);
        pdfStream.Position = 0;
        await s3Service.UploadFileAttach(pdfStream, $"Upload/ElectProjects/{electProjectErtForm.ElectProject.Id}/approved-ert-form.pdf");
        //-----------------------

        return "ok";
    }

    public async Task<string> GetApprovedSentToElectForm(byte[] assign, ElectProject electProject, string engName, string sendToElect)
    {

        var webRootPath = hostingEnvironment.ContentRootPath;
        var mrtPath = Path.Combine(webRootPath, "Upload", "Reports", "ApprovedSendToElectForm.mrt");

        var report = new StiReport();
        var pdfSettings = new StiPdfExportSettings
        {
            EmbeddedFonts = true
        };

        // loaded report
        report.Load(mrtPath);
        report.Dictionary.Databases.Clear();
        var data = new DataSet("Demo");
        var dataTable = Helper.LinqToDataTable(electProject);
        data.Tables.Add(dataTable);
        report.Dictionary.Databases.Clear();

        report.Dictionary.Variables["engName"].Value = engName;
        report.Dictionary.Variables["solarDate"].Value = Helper.MiladiToShamsi(DateTime.Now.Date);
        report.Dictionary.Variables["landlordName"].Value = Helper.RemoveDiacritics(electProject.LandlordName);
        report.Dictionary.Variables["address"].Value = Helper.RemoveDiacritics( electProject.Address);
        report.Dictionary.Variables["fileNumber"].Value = electProject.FileNumber.ToString();
        report.Dictionary.Variables["sendToElect"].Value = sendToElect;
        report.Dictionary.Variables["solarRegisterElectProject"].Value = electProject.SolarRegisterDate;
        report.Dictionary.Variables["requestNumber"].Value = electProject.ElectRequestNumber.ToString();
        report.Dictionary.Variables["needElectNetwork"].Value = electProject.NeedElectNetwork.ToString();



		// Add Eng Assign to report
		if (!report.Dictionary.Resources.Contains("adminAssign"))
            report.Dictionary.Resources.Add(new StiResource("adminAssign", StiResourceType.Image, assign));
        else
            report.Dictionary.Resources["adminAssign"].Content = assign;
        // -------------------------

        
        // Register data to report and render
        report.RegData("Demo", "Demo", dataTable);
        if (report.Dictionary.DataSources.Count == 0)
        {
            throw new NotFoundException("خطا در ایجاد گزارش");
        }

        await report.RenderAsync();
        //--------------------

        // Pdf
        using var pdfStream = new MemoryStream();
       
		await report.ExportDocumentAsync(StiExportFormat.Pdf, pdfStream, pdfSettings);

        pdfStream.Position = 0;
        await s3Service.UploadFileAttach(pdfStream, $"Upload/ElectProjects/{electProject.Id}/approved-send-to-elect.pdf");
        //-----------------------

        return "ok";

    }
}