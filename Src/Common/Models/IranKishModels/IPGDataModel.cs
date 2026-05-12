using System.Collections.Generic;

namespace Coreapi.Common.Models.IranKishModels;

public class IPGDataModel
{
    private string terminalId;

    private string acceptorId;

    private string passPhrase;

    private string revertURL;

    private string rsaPublicKey;

    private long amount;

    private string paymentId;

    private string requestId;

    private string cmsPreservationId;

    private List<MultiplexParameter> multiplexParameters;



    // کد پایانه 
    //[XmlElement("TerminalId")]
    public string TerminalId { get; set; }

    // کد پذیرنده 
    public string AcceptorId { get; set; }
    // عبارت عبور
    public string PassPhrase { get; set; }
    // مقدار-ریال
    public long Amount { get; set; }
    //{

    //    get { return amount; }

    //    set { amount = long.Parse(amount) }

    //}
    //Hard Code ip in xml file
    public string RevertURL { get; set; }
    public string PaymentId { get; set; }
    public string RequestId { get; set; }
    public string CmsPreservationId { get; set; }
    public string TransactionType { get; set; }

    public BillInfo BillInfo { get; set; }

    public string RsaPublicKey { get; set; }
    public List<MultiplexParameter> MultiplexParameters { get; set; }
}