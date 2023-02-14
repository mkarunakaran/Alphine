////using AutoMapper;
////using Microsoft.AspNetCore.Mvc;
////using Sumday.BoundedContext.ShareHolder;
////using Sumday.Domain.Abstractions.EntryPorts;
////using Sumday.Infrastructure.Common;
////using System.Threading;
////using System.Threading.Tasks;

////namespace Sumday.Service.ShareHolder.Payees
////{
////    [Route("api/accounts/{accountId}/bank-accounts")]
////    [ApiController]
////    public class BankPayeeController : ControllerBase
////    {
////        private readonly ICallContext callContext;
////        private readonly IQueryUseCaseInteractor queryUseCaseInteractor;
////        private readonly ICommandUseCaseInteractor commandUseCaseInteractor;
////        private readonly IMapper mapper;
  
////        public BankPayeeController(ICallContext callContext, ICommandUseCaseInteractor commandUseCaseInteractor, IQueryUseCaseInteractor queryUseCaseInteractor, IMapper mapper)
////        {
////            this.callContext = callContext;
////            this.queryUseCaseInteractor = queryUseCaseInteractor;
////            this.commandUseCaseInteractor = commandUseCaseInteractor;
////            this.mapper = mapper;
////        }

////        [HttpGet("")]
////        public async Task<IActionResult> GetAll(string accountId, CancellationToken cancellationToken)
////        {
////            var query = new GetSharedHolderProfileQuery("XVZF4GM7CUCGLPWXSBINJW");

////            var outPutPort = new BankPayeePresenter(this.mapper);
////            var shareHolderProfileQueryUseCase = new QueryUseCase<GetSharedHolderProfileQuery, ShareHolderInstance>(query, outPutPort);
////            _ = await this.queryUseCaseInteractor.Send(shareHolderProfileQueryUseCase, HttpContext.RequestAborted);

////            var account = await this.GetAccountById(accountId);

////            if (account == null)
////            {
////                return this.NotFound();
////            }

////            var bankAccounts = await this.GetAccountBankAccounts(account.AccountNumber);

////            var plaidCalls = bankAccounts.Where(bnk => !string.IsNullOrEmpty(bnk.RoutingNumber)).Select(bnk => this.queryPipe.Send(new GetPlaidInstitutionQuery { RoutingNumber = bnk.RoutingNumber }, cancellationToken)).ToList();
////            await Task.WhenAll(plaidCalls);
////            bankAccounts.ForEach(bnk => bnk.Logo = plaidCalls.FirstOrDefault(pld => pld.Result?.RoutingNumbers?.Contains(bnk.RoutingNumber) ?? false)?.Result?.Logo);

////            return this.Ok(bankAccounts);
////        }

////        [HttpGet("{bankAccountId}")]
////        public async Task<IActionResult> Get(string accountId, string bankAccountId, CancellationToken cancellationToken)
////        {
////            var account = await this.GetAccountById(accountId);

////            if (account == null)
////            {
////                return this.NotFound();
////            }

////            var bankAccounts = await this.GetAccountBankAccounts(account.AccountNumber);
////            var bankAccount = bankAccounts.SingleOrDefault(i => i.Id == bankAccountId);

////            if (bankAccount == null)
////            {
////                return this.NotFound();
////            }

////            if (!string.IsNullOrEmpty(bankAccount.RoutingNumber))
////            {
////                var plaidInstitution = await this.queryPipe.Send(new GetPlaidInstitutionQuery { RoutingNumber = bankAccount.RoutingNumber }, cancellationToken);
////                bankAccount.Logo = plaidInstitution?.Logo;
////            }

////            return this.Ok(bankAccount);
////        }
////    }
////}
