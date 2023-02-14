using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sumday.BoundedContext.ShareHolder;
using Sumday.BoundedContext.ShareHolder.Accounts;
using Sumday.Domain.Abstractions.EntryPorts;
using Sumday.Service.ShareHolder.ShareHolderInfo.ViewModels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sumday.Service.ShareHolder.Accounts
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IQueryUseCaseInteractor queryUseCaseInteractor;
        private readonly IMapper mapper;
        public AccountsController(IQueryUseCaseInteractor queryUseCaseInteractor, IMapper mapper)
        {
            this.queryUseCaseInteractor = queryUseCaseInteractor;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken stoppingToken)
        {
            var query = new GetSharedHolderProfileQuery("XVZF4GM7CUCGLPWXSBINJW");
          
            var shareHolderPresenter = new QueryPresenter<ShareHolderInstance, Customer>(this.mapper);
            var shareHolderProfileQueryUseCase = new QueryUseCase<GetSharedHolderProfileQuery, ShareHolderInstance>(query, shareHolderPresenter);
            var result = await this.queryUseCaseInteractor.Send(shareHolderProfileQueryUseCase, HttpContext.RequestAborted);
            if (result.IsSuccess)
            {
                var getAllaccountsquery = new GetAllAccountsQuery(shareHolderPresenter.Payload.Id);
                var accountsPresenter = new QueryPresenter<List<AccountInstance>, List<Account>>(this.mapper);
                var allAccountsUsecase = new QueryUseCase<GetAllAccountsQuery, List<AccountInstance>>(getAllaccountsquery, accountsPresenter);
                _ = await queryUseCaseInteractor.Send(allAccountsUsecase, stoppingToken);
                return accountsPresenter.ViewModel;
            }

            return shareHolderPresenter.ViewModel;
        }
    }
}
