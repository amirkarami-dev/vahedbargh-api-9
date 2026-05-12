using System;
using System.Threading.Tasks;
using Coreapi.Application.Features.Engineers.Queries.GetListEngWork;
using Coreapi.Application.Features.Transactions.Commands.EngPayment;
using Coreapi.Application.Features.Transactions.Commands.EngPaymentApproved;
using Coreapi.Application.Features.Transactions.Commands.EngPaymentCustom;
using Coreapi.Application.Features.Transactions.Commands.PaymentCustom;
using Coreapi.Application.Features.Transactions.Commands.PaymentMelli;
using Coreapi.Application.Features.Transactions.Commands.PaymentMelliPublic;
using Coreapi.Application.Features.Transactions.Commands.PaymentMelliPublicReturn;
using Coreapi.Application.Features.Transactions.Commands.PaymentMelliReturn;
using Coreapi.Application.Features.Transactions.Commands.UpdateEngPayment;
using Coreapi.Application.Features.Transactions.Queries.GetClientEngInvoiceReport;
using Coreapi.Application.Features.Transactions.Queries.GetClientInvoices;
using Coreapi.Application.Features.Transactions.Queries.GetClientUserTransactions;
using Coreapi.Application.Features.Transactions.Queries.GetEngClientInvoices;
using Coreapi.Application.Features.Transactions.Queries.GetEngPayments;
using Coreapi.Application.Features.Transactions.Queries.GetEngPaymentTasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coreapi.Api.Controllers
{

    [Authorize]
    public class TransactionsController : BaseController
    {



         [HttpPost]
         [Authorize(Roles = "Accountant, Administrator, Section")]
        public async Task<IActionResult> PaymentCustom(PaymentCustomCommand command) =>
             Ok(await Mediator.Send(command));

        [HttpPost]
        [Authorize(Roles = "Accountant")]
        public async Task<IActionResult> EngPaymentCustom(EngPaymentCustomCommand command) =>
            Ok(await Mediator.Send(command));


      
        [HttpPost]
        // برای ایجاد توکن مبلغ و شماره پرونده ارسال میشه
        public async Task<IActionResult> PaymentMelli(PaymentMelliCommand command) => Ok(await Mediator.Send(command));

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PaymentMelliReturn([FromForm] PaymentMelliReturnCommand command)
        {
        
            var result = await Mediator.Send(command);


            var redirectResult = new RedirectResult(result, true);
            return redirectResult;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PaymentMelliPublicReturn([FromForm] PaymentMelliPublicReturnCommand command)
        {

            var result = await Mediator.Send(command);


            var redirectResult = new RedirectResult(result, true);
            return redirectResult;
        }

        [HttpPost]
        public async Task<IActionResult> GetClientUserTransactions([FromBody] GetClientUserTransactionQuery query)=> Ok(await Mediator.Send(query));

        [HttpGet]
        [Authorize(Roles = "Accountant")]
        public async Task<IActionResult> GetClientInvoices([FromQuery] GetClientInvoiceQuery query) => Ok(await Mediator.Send(query));

        [HttpGet]
        [Authorize(Roles = "Accountant, Administrator, Engineer")]
        public async Task<IActionResult> GetClientEngWork([FromQuery] GetListEngWorkQuery query) => Ok(await Mediator.Send(query));

        [HttpGet]
        [Authorize(Roles = "Engineer")]
        public async Task<IActionResult> GetEngClientInvoices([FromQuery] GetEngClientInvoiceQuery query) => Ok(await Mediator.Send(query));

        [HttpGet]
        [Authorize(Roles = "Accountant")]
        public async Task<IActionResult> GetClientEngInvoiceReport([FromQuery] EngInvoiceReportQuery query) => Ok(await Mediator.Send(query));

        [HttpPost]
        [Authorize(Roles = "Accountant")]
        public async Task<IActionResult> UpsertEngPaymentList([FromBody] EngPaymentListCommand command) => Ok(await Mediator.Send(command));

        [HttpGet]
        [Authorize(Roles = "Accountant")]
        public async Task<IActionResult> GetEngPaymentList([FromQuery] GetEngPaymentQuery query) =>
            Ok(await Mediator.Send(query));

        [HttpPost]
        [Authorize(Roles = "Accountant")]
        public async Task<IActionResult> UpdateEngPaymentList([FromBody] UpdateEngPaymentCommand command) =>
            Ok(await Mediator.Send(command));

        [HttpGet]
        [Authorize(Roles = "Accountant")]
        public async Task<IActionResult> GetEngPaymentTasks([FromQuery] GetEngPaymentTasksQuery query) =>
            Ok(await Mediator.Send(query));

        [HttpPost]
        [Authorize(Roles = "Accountant")]
        public async Task<IActionResult> EngPaymentApproved([FromBody] EngPaymentApprovedCommand command) =>
            Ok(await Mediator.Send(command));
    }
}
