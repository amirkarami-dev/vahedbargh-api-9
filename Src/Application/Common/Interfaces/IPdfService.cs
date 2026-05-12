using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Coreapi.Application.Common.Interfaces
{
    public interface IPdfService
    {
        Task<string> SignatureToPdf(string pathPdfFile, string pathSignatureImage, string pathOutputName, int paddingTop, int paddingRight);
        Task<string> SignatureToImage(string pathImageFile, string pathSignatureImage, string pathOutputName);
        Task<bool> SignatureToElectProjectImage(string mapFrontPath, string backFrontPath, string signEngineerPath, string signExecutorPath, string signNezam, string signEjraii, string mapFrontPathOutput, string backFrontPathOutput, string textDes, string textEngDes, string txtMapFrontSubmit);
    }
}