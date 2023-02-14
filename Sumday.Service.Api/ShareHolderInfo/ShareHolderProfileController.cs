using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sumday.BoundedContext.ShareHolder;
using Sumday.Domain.Abstractions.EntryPorts;
using Sumday.Service.ShareHolder.ShareHolderInfo.Presenters;
using Sumday.Service.ShareHolder.ShareHolderInfo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sumday.Service.ShareHolder.ShareHolderInfo
{
    [Route("api/customer")]
    [ApiController]
    public class ShareHolderProfileController : ControllerBase
    {
        private readonly IQueryUseCaseInteractor queryUseCaseInteractor;
        private readonly IMapper mapper;
        public ShareHolderProfileController(IQueryUseCaseInteractor queryUseCaseInteractor, IMapper mapper)
        {
            this.queryUseCaseInteractor = queryUseCaseInteractor;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = new GetSharedHolderProfileQuery("XVZF4GM7CUCGLPWXSBINJW");

            var shareHolderPresenter = new QueryPresenter<ShareHolderInstance, Customer>(this.mapper);
            var shareHolderProfileQueryUseCase = new QueryUseCase<GetSharedHolderProfileQuery, ShareHolderInstance>(query, shareHolderPresenter);
            _ = await this.queryUseCaseInteractor.Send(shareHolderProfileQueryUseCase, HttpContext.RequestAborted);
            return shareHolderPresenter.ViewModel;

        }
    }
}
