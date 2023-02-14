using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sumday.Domain.Abstractions.EntryPorts;

namespace Sumday.Service.ShareHolder
{
    public class CommandPresenter<T> : ICommandOutputPort<T>
    {
        public IActionResult ViewModel { get; private set; }
        public void Output(UseCaseResult<T> interactorOutput)
        {
            if (interactorOutput.IsSuccessful)
            {
                 ViewModel = new OkObjectResult(interactorOutput.Payload);
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
