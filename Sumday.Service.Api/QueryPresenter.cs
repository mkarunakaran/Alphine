using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sumday.Domain.Abstractions.EntryPorts;

namespace Sumday.Service.ShareHolder
{
    public class QueryPresenter<T, Tto> : IQueryOutputPort<T>
    {
        private readonly IMapper mapper;

        public QueryPresenter(IMapper mapper)
        {
            this.mapper = mapper;
        }
        public IActionResult ViewModel { get; private set; }

        public Tto Payload { get; private set; }
        public void Output(UseCaseResult<T> interactorOutput)
        {
            if (interactorOutput.IsSuccessful)
            {
                this.Payload = this.mapper.Map<Tto>(interactorOutput.Payload);
                ViewModel = new OkObjectResult(this.Payload);
            }
            else if (interactorOutput.ResultCategory == ResultCategory.NotFound)
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
