using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sumday.BoundedContext.ShareHolder;
using Sumday.Domain.Abstractions.EntryPorts;
using Sumday.Service.ShareHolder.ShareHolderInfo.ViewModels;

namespace Sumday.Service.ShareHolder.ShareHolderInfo.Presenters
{
    public class ShareHolderProfiePresenter : IQueryOutputPort<ShareHolderInstance>
    {
        private readonly IMapper mapper;

        public ShareHolderProfiePresenter(IMapper mapper)
        {
            this.mapper = mapper;
        }
        public IActionResult ViewModel { get; private set; }
        public void Output(UseCaseResult<ShareHolderInstance> interactorOutput)
        {
            if(interactorOutput.IsSuccessful)
            {
                var customer = this.mapper.Map<Customer>(interactorOutput.Payload);

                ViewModel = new OkObjectResult(customer);
            }
            else if(interactorOutput.ResultCategory == ResultCategory.NotFound)
            {
                ViewModel = new NotFoundObjectResult(interactorOutput.ErrorMessage);
            }
            else
            {
                ViewModel = new BadRequestObjectResult(interactorOutput.ErrorMessage);
            }
        }
    }
}
