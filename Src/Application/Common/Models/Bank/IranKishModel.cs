using Coreapi.Common.Models.IranKishModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace Coreapi.Application.Common.Models.Bank;

public class TokenResult
{
    public TokenResult()
    {
        result = new Result();
    }

    public string responseCode { get; set; }
    public object description { get; set; }
    public bool status { get; set; }
    public Result result { get; set; }
}

public class VerifyResult
{
    public string responseCode { get; set; }
    public string description { get; set; }
    public bool status { get; set; }
    public SubResult result { get; set; }



}
public class CallbackRequestPaymentIranKishModel
{
    public string PrimaryAccNo { get; set; }
    public string HashedCardNo { get; set; }
    public long OrderId { get; set; }
    public string SwitchResCode { get; set; }
    public int ResCode { get; set; }
    public string Token { get; set; }
}

public class VerifyResponsePaymentIranKishModel
{
    public int ResCode { get; set; }
    public int? Amount { get; set; }
    public string Description { get; set; }
    public string RetrivalRefNo { get; set; }
    public string SystemTraceNo { get; set; }
    public long? OrderId { get; set; }
}

public class SubResult
{

    public string responseCode { get; set; }
    public string systemTraceAuditNumber { get; set; }
    public string retrievalReferenceNumber { get; set; }
    public DateTime transactionDateTime { get; set; }
    public int transactionDate { get; set; }
    public int transactionTime { get; set; }
    public string processCode { get; set; }
    public object requestId { get; set; }
    public object additional { get; set; }
    public object billType { get; set; }
    public object billId { get; set; }
    public string paymentId { get; set; }
    public string amount { get; set; }
    public object revertUri { get; set; }
    public object acceptorId { get; set; }
    public object terminalId { get; set; }
    public object tokenIdentity { get; set; }

}


public class RequestVerify
{

    //شماره ترمینال
    public string TerminalId { get; set; }

    //شماره ارجاع
    public string RetrievalReferenceNumber { get; set; }

    //شماره سند/ پیگیری
    public string SystemTraceAuditNumber { get; set; }

    //توکن
    public string TokenIdentity { get; set; }



}