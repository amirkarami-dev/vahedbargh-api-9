using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Common.Models
{
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpVerifyRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpVerifyRequest", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpVerifyRequestBody Body;

        public bpVerifyRequest()
        {
        }

        public bpVerifyRequest(bpVerifyRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpVerifyRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(Order = 0)]
        public long terminalId;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string userName;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string userPassword;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 3)]
        public long orderId;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 4)]
        public long saleOrderId;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 5)]
        public long saleReferenceId;

        public bpVerifyRequestBody()
        {
        }

        public bpVerifyRequestBody(long terminalId, string userName, string userPassword, long orderId, long saleOrderId, long saleReferenceId)
        {
            this.terminalId = terminalId;
            this.userName = userName;
            this.userPassword = userPassword;
            this.orderId = orderId;
            this.saleOrderId = saleOrderId;
            this.saleReferenceId = saleReferenceId;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpVerifyRequestResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpVerifyRequestResponse", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpVerifyRequestResponseBody Body;

        public bpVerifyRequestResponse()
        {
        }

        public bpVerifyRequestResponse(bpVerifyRequestResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpVerifyRequestResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string @return;

        public bpVerifyRequestResponseBody()
        {
        }

        public bpVerifyRequestResponseBody(string @return)
        {
            this.@return = @return;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpRefundInquiryRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpRefundInquiryRequest", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpRefundInquiryRequestBody Body;

        public bpRefundInquiryRequest()
        {
        }

        public bpRefundInquiryRequest(bpRefundInquiryRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpRefundInquiryRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(Order = 0)]
        public long terminalId;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string userName;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string userPassword;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 3)]
        public long orderId;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 4)]
        public long refundOrderId;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 5)]
        public long refundReferenceId;

        public bpRefundInquiryRequestBody()
        {
        }

        public bpRefundInquiryRequestBody(long terminalId, string userName, string userPassword, long orderId, long refundOrderId, long refundReferenceId)
        {
            this.terminalId = terminalId;
            this.userName = userName;
            this.userPassword = userPassword;
            this.orderId = orderId;
            this.refundOrderId = refundOrderId;
            this.refundReferenceId = refundReferenceId;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpRefundInquiryRequestResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpRefundInquiryRequestResponse", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpRefundInquiryRequestResponseBody Body;

        public bpRefundInquiryRequestResponse()
        {
        }

        public bpRefundInquiryRequestResponse(bpRefundInquiryRequestResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpRefundInquiryRequestResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string @return;

        public bpRefundInquiryRequestResponseBody()
        {
        }

        public bpRefundInquiryRequestResponseBody(string @return)
        {
            this.@return = @return;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpRefundVerifyRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpRefundVerifyRequest", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpRefundVerifyRequestBody Body;

        public bpRefundVerifyRequest()
        {
        }

        public bpRefundVerifyRequest(bpRefundVerifyRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpRefundVerifyRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(Order = 0)]
        public long terminalId;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string userName;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string userPassword;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 3)]
        public long orderId;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 4)]
        public long refundOrderId;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 5)]
        public long refundReferenceId;

        public bpRefundVerifyRequestBody()
        {
        }

        public bpRefundVerifyRequestBody(long terminalId, string userName, string userPassword, long orderId, long refundOrderId, long refundReferenceId)
        {
            this.terminalId = terminalId;
            this.userName = userName;
            this.userPassword = userPassword;
            this.orderId = orderId;
            this.refundOrderId = refundOrderId;
            this.refundReferenceId = refundReferenceId;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpRefundVerifyRequestResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpRefundVerifyRequestResponse", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpRefundVerifyRequestResponseBody Body;

        public bpRefundVerifyRequestResponse()
        {
        }

        public bpRefundVerifyRequestResponse(bpRefundVerifyRequestResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpRefundVerifyRequestResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string @return;

        public bpRefundVerifyRequestResponseBody()
        {
        }

        public bpRefundVerifyRequestResponseBody(string @return)
        {
            this.@return = @return;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpSettleRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpSettleRequest", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpSettleRequestBody Body;

        public bpSettleRequest()
        {
        }

        public bpSettleRequest(bpSettleRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpSettleRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(Order = 0)]
        public long terminalId;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string userName;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string userPassword;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 3)]
        public long orderId;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 4)]
        public long saleOrderId;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 5)]
        public long saleReferenceId;

        public bpSettleRequestBody()
        {
        }

        public bpSettleRequestBody(long terminalId, string userName, string userPassword, long orderId, long saleOrderId, long saleReferenceId)
        {
            this.terminalId = terminalId;
            this.userName = userName;
            this.userPassword = userPassword;
            this.orderId = orderId;
            this.saleOrderId = saleOrderId;
            this.saleReferenceId = saleReferenceId;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpSettleRequestResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpSettleRequestResponse", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpSettleRequestResponseBody Body;

        public bpSettleRequestResponse()
        {
        }

        public bpSettleRequestResponse(bpSettleRequestResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpSettleRequestResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string @return;

        public bpSettleRequestResponseBody()
        {
        }

        public bpSettleRequestResponseBody(string @return)
        {
            this.@return = @return;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpDynamicPayRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpDynamicPayRequest", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpDynamicPayRequestBody Body;

        public bpDynamicPayRequest()
        {
        }

        public bpDynamicPayRequest(bpDynamicPayRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpDynamicPayRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(Order = 0)]
        public long terminalId;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string userName;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string userPassword;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 3)]
        public long orderId;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 4)]
        public long amount;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string localDate;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string localTime;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string additionalData;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
        public string callBackUrl;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 9)]
        public long payerId;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 10)]
        public long subServiceId;

        public bpDynamicPayRequestBody()
        {
        }

        public bpDynamicPayRequestBody(long terminalId, string userName, string userPassword, long orderId, long amount, string localDate, string localTime, string additionalData, string callBackUrl, long payerId, long subServiceId)
        {
            this.terminalId = terminalId;
            this.userName = userName;
            this.userPassword = userPassword;
            this.orderId = orderId;
            this.amount = amount;
            this.localDate = localDate;
            this.localTime = localTime;
            this.additionalData = additionalData;
            this.callBackUrl = callBackUrl;
            this.payerId = payerId;
            this.subServiceId = subServiceId;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpDynamicPayRequestResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpDynamicPayRequestResponse", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpDynamicPayRequestResponseBody Body;

        public bpDynamicPayRequestResponse()
        {
        }

        public bpDynamicPayRequestResponse(bpDynamicPayRequestResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpDynamicPayRequestResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string @return;

        public bpDynamicPayRequestResponseBody()
        {
        }

        public bpDynamicPayRequestResponseBody(string @return)
        {
            this.@return = @return;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpVirtualPayRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpVirtualPayRequest", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpVirtualPayRequestBody Body;

        public bpVirtualPayRequest()
        {
        }

        public bpVirtualPayRequest(bpVirtualPayRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpVirtualPayRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(Order = 0)]
        public long terminalId;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string userName;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string userPassword;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 3)]
        public long orderId;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 4)]
        public long amount;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string localDate;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string localTime;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string additionalData;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
        public string callBackUrl;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 9)]
        public long payerId;

        public bpVirtualPayRequestBody()
        {
        }

        public bpVirtualPayRequestBody(long terminalId, string userName, string userPassword, long orderId, long amount, string localDate, string localTime, string additionalData, string callBackUrl, long payerId)
        {
            this.terminalId = terminalId;
            this.userName = userName;
            this.userPassword = userPassword;
            this.orderId = orderId;
            this.amount = amount;
            this.localDate = localDate;
            this.localTime = localTime;
            this.additionalData = additionalData;
            this.callBackUrl = callBackUrl;
            this.payerId = payerId;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpVirtualPayRequestResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpVirtualPayRequestResponse", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpVirtualPayRequestResponseBody Body;

        public bpVirtualPayRequestResponse()
        {
        }

        public bpVirtualPayRequestResponse(bpVirtualPayRequestResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpVirtualPayRequestResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string @return;

        public bpVirtualPayRequestResponseBody()
        {
        }

        public bpVirtualPayRequestResponseBody(string @return)
        {
            this.@return = @return;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpReversalRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpReversalRequest", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpReversalRequestBody Body;

        public bpReversalRequest()
        {
        }

        public bpReversalRequest(bpReversalRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpReversalRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(Order = 0)]
        public long terminalId;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string userName;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string userPassword;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 3)]
        public long orderId;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 4)]
        public long saleOrderId;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 5)]
        public long saleReferenceId;

        public bpReversalRequestBody()
        {
        }

        public bpReversalRequestBody(long terminalId, string userName, string userPassword, long orderId, long saleOrderId, long saleReferenceId)
        {
            this.terminalId = terminalId;
            this.userName = userName;
            this.userPassword = userPassword;
            this.orderId = orderId;
            this.saleOrderId = saleOrderId;
            this.saleReferenceId = saleReferenceId;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpReversalRequestResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpReversalRequestResponse", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpReversalRequestResponseBody Body;

        public bpReversalRequestResponse()
        {
        }

        public bpReversalRequestResponse(bpReversalRequestResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpReversalRequestResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string @return;

        public bpReversalRequestResponseBody()
        {
        }

        public bpReversalRequestResponseBody(string @return)
        {
            this.@return = @return;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpCumulativeDynamicPayRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpCumulativeDynamicPayRequest", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpCumulativeDynamicPayRequestBody Body;

        public bpCumulativeDynamicPayRequest()
        {
        }

        public bpCumulativeDynamicPayRequest(bpCumulativeDynamicPayRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpCumulativeDynamicPayRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(Order = 0)]
        public long terminalId;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string userName;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string userPassword;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 3)]
        public long orderId;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 4)]
        public long amount;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string localDate;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string localTime;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string additionalData;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
        public string callBackUrl;

        public bpCumulativeDynamicPayRequestBody()
        {
        }

        public bpCumulativeDynamicPayRequestBody(long terminalId, string userName, string userPassword, long orderId, long amount, string localDate, string localTime, string additionalData, string callBackUrl)
        {
            this.terminalId = terminalId;
            this.userName = userName;
            this.userPassword = userPassword;
            this.orderId = orderId;
            this.amount = amount;
            this.localDate = localDate;
            this.localTime = localTime;
            this.additionalData = additionalData;
            this.callBackUrl = callBackUrl;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpCumulativeDynamicPayRequestResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpCumulativeDynamicPayRequestResponse", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpCumulativeDynamicPayRequestResponseBody Body;

        public bpCumulativeDynamicPayRequestResponse()
        {
        }

        public bpCumulativeDynamicPayRequestResponse(bpCumulativeDynamicPayRequestResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpCumulativeDynamicPayRequestResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string @return;

        public bpCumulativeDynamicPayRequestResponseBody()
        {
        }

        public bpCumulativeDynamicPayRequestResponseBody(string @return)
        {
            this.@return = @return;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpPayRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpPayRequest", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpPayRequestBody Body;

        public bpPayRequest()
        {
        }

        public bpPayRequest(bpPayRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpPayRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(Order = 0)]
        public long terminalId;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string userName;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string userPassword;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 3)]
        public long orderId;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 4)]
        public long amount;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string localDate;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string localTime;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string additionalData;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
        public string callBackUrl;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 9)]
        public long payerId;

        public bpPayRequestBody()
        {
        }

        public bpPayRequestBody(long terminalId, string userName, string userPassword, long orderId, long amount, string localDate, string localTime, string additionalData, string callBackUrl, long payerId)
        {
            this.terminalId = terminalId;
            this.userName = userName;
            this.userPassword = userPassword;
            this.orderId = orderId;
            this.amount = amount;
            this.localDate = localDate;
            this.localTime = localTime;
            this.additionalData = additionalData;
            this.callBackUrl = callBackUrl;
            this.payerId = payerId;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpPayRequestResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpPayRequestResponse", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpPayRequestResponseBody Body;

        public bpPayRequestResponse()
        {
        }

        public bpPayRequestResponse(bpPayRequestResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpPayRequestResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string @return;

        public bpPayRequestResponseBody()
        {
        }

        public bpPayRequestResponseBody(string @return)
        {
            this.@return = @return;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpSaleReferenceIdRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpSaleReferenceIdRequest", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpSaleReferenceIdRequestBody Body;

        public bpSaleReferenceIdRequest()
        {
        }

        public bpSaleReferenceIdRequest(bpSaleReferenceIdRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpSaleReferenceIdRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(Order = 0)]
        public long terminalId;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string userName;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string userPassword;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 3)]
        public long orderId;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 4)]
        public long saleOrderId;

        public bpSaleReferenceIdRequestBody()
        {
        }

        public bpSaleReferenceIdRequestBody(long terminalId, string userName, string userPassword, long orderId, long saleOrderId)
        {
            this.terminalId = terminalId;
            this.userName = userName;
            this.userPassword = userPassword;
            this.orderId = orderId;
            this.saleOrderId = saleOrderId;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpSaleReferenceIdRequestResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpSaleReferenceIdRequestResponse", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpSaleReferenceIdRequestResponseBody Body;

        public bpSaleReferenceIdRequestResponse()
        {
        }

        public bpSaleReferenceIdRequestResponse(bpSaleReferenceIdRequestResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpSaleReferenceIdRequestResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string @return;

        public bpSaleReferenceIdRequestResponseBody()
        {
        }

        public bpSaleReferenceIdRequestResponseBody(string @return)
        {
            this.@return = @return;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpInquiryRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpInquiryRequest", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpInquiryRequestBody Body;

        public bpInquiryRequest()
        {
        }

        public bpInquiryRequest(bpInquiryRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpInquiryRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(Order = 0)]
        public long terminalId;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string userName;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string userPassword;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 3)]
        public long orderId;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 4)]
        public long saleOrderId;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 5)]
        public long saleReferenceId;

        public bpInquiryRequestBody()
        {
        }

        public bpInquiryRequestBody(long terminalId, string userName, string userPassword, long orderId, long saleOrderId, long saleReferenceId)
        {
            this.terminalId = terminalId;
            this.userName = userName;
            this.userPassword = userPassword;
            this.orderId = orderId;
            this.saleOrderId = saleOrderId;
            this.saleReferenceId = saleReferenceId;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpInquiryRequestResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpInquiryRequestResponse", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpInquiryRequestResponseBody Body;

        public bpInquiryRequestResponse()
        {
        }

        public bpInquiryRequestResponse(bpInquiryRequestResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpInquiryRequestResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string @return;

        public bpInquiryRequestResponseBody()
        {
        }

        public bpInquiryRequestResponseBody(string @return)
        {
            this.@return = @return;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpChargePayRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpChargePayRequest", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpChargePayRequestBody Body;

        public bpChargePayRequest()
        {
        }

        public bpChargePayRequest(bpChargePayRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpChargePayRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(Order = 0)]
        public long terminalId;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string userName;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string userPassword;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 3)]
        public long orderId;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 4)]
        public long amount;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string localDate;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string localTime;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string additionalData;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
        public string callBackUrl;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 9)]
        public long payerId;

        public bpChargePayRequestBody()
        {
        }

        public bpChargePayRequestBody(long terminalId, string userName, string userPassword, long orderId, long amount, string localDate, string localTime, string additionalData, string callBackUrl, long payerId)
        {
            this.terminalId = terminalId;
            this.userName = userName;
            this.userPassword = userPassword;
            this.orderId = orderId;
            this.amount = amount;
            this.localDate = localDate;
            this.localTime = localTime;
            this.additionalData = additionalData;
            this.callBackUrl = callBackUrl;
            this.payerId = payerId;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpChargePayRequestResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpChargePayRequestResponse", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpChargePayRequestResponseBody Body;

        public bpChargePayRequestResponse()
        {
        }

        public bpChargePayRequestResponse(bpChargePayRequestResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpChargePayRequestResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string @return;

        public bpChargePayRequestResponseBody()
        {
        }

        public bpChargePayRequestResponseBody(string @return)
        {
            this.@return = @return;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpPosRefundRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpPosRefundRequest", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpPosRefundRequestBody Body;

        public bpPosRefundRequest()
        {
        }

        public bpPosRefundRequest(bpPosRefundRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpPosRefundRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string user;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string password;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 2)]
        public long saleReferenceId;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 3)]
        public long refundAmount;

        public bpPosRefundRequestBody()
        {
        }

        public bpPosRefundRequestBody(string user, string password, long saleReferenceId, long refundAmount)
        {
            this.user = user;
            this.password = password;
            this.saleReferenceId = saleReferenceId;
            this.refundAmount = refundAmount;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpPosRefundRequestResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpPosRefundRequestResponse", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpPosRefundRequestResponseBody Body;

        public bpPosRefundRequestResponse()
        {
        }

        public bpPosRefundRequestResponse(bpPosRefundRequestResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpPosRefundRequestResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string @return;

        public bpPosRefundRequestResponseBody()
        {
        }

        public bpPosRefundRequestResponseBody(string @return)
        {
            this.@return = @return;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpRefundToPANRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpRefundToPANRequest", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpRefundToPANRequestBody Body;

        public bpRefundToPANRequest()
        {
        }

        public bpRefundToPANRequest(bpRefundToPANRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpRefundToPANRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string user;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string password;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 2)]
        public long pan;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 3)]
        public long amount;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 4)]
        public long terminalId;

        public bpRefundToPANRequestBody()
        {
        }

        public bpRefundToPANRequestBody(string user, string password, long pan, long amount, long terminalId)
        {
            this.user = user;
            this.password = password;
            this.pan = pan;
            this.amount = amount;
            this.terminalId = terminalId;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpRefundToPANRequestResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpRefundToPANRequestResponse", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpRefundToPANRequestResponseBody Body;

        public bpRefundToPANRequestResponse()
        {
        }

        public bpRefundToPANRequestResponse(bpRefundToPANRequestResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpRefundToPANRequestResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string @return;

        public bpRefundToPANRequestResponseBody()
        {
        }

        public bpRefundToPANRequestResponseBody(string @return)
        {
            this.@return = @return;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpRefundRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpRefundRequest", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpRefundRequestBody Body;

        public bpRefundRequest()
        {
        }

        public bpRefundRequest(bpRefundRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpRefundRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(Order = 0)]
        public long terminalId;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string userName;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string userPassword;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 3)]
        public long orderId;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 4)]
        public long saleOrderId;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 5)]
        public long saleReferenceId;

        [System.Runtime.Serialization.DataMemberAttribute(Order = 6)]
        public long refundAmount;

        public bpRefundRequestBody()
        {
        }

        public bpRefundRequestBody(long terminalId, string userName, string userPassword, long orderId, long saleOrderId, long saleReferenceId, long refundAmount)
        {
            this.terminalId = terminalId;
            this.userName = userName;
            this.userPassword = userPassword;
            this.orderId = orderId;
            this.saleOrderId = saleOrderId;
            this.saleReferenceId = saleReferenceId;
            this.refundAmount = refundAmount;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class bpRefundRequestResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "bpRefundRequestResponse", Namespace = "http://interfaces.core.sw.bps.com/", Order = 0)]
        public bpRefundRequestResponseBody Body;

        public bpRefundRequestResponse()
        {
        }

        public bpRefundRequestResponse(bpRefundRequestResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "")]
    public partial class bpRefundRequestResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string @return;

        public bpRefundRequestResponseBody()
        {
        }

        public bpRefundRequestResponseBody(string @return)
        {
            this.@return = @return;
        }
    }
}
