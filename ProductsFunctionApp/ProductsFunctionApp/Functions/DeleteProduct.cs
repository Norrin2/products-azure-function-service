using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ProductsFunctionApp.Dto;
using ProductsFunctionApp.Repository;
using ProductsFunctionApp.Validation;
using System.Threading.Tasks;
using ProductsFunctionApp.Utils;

namespace ProductsFunctionApp.Functions
{
    public static class DeleteProduct
    {
        [FunctionName("DeleteProduct")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "products")] HttpRequest req,
            ILogger log)
        {
            int.TryParse(req.Query["id"], out int id);
            int.TryParse(req.Query["companyId"], out int companyId);

            var dto = new FindProductDto
            {
                Id = id,
                CompanyId = companyId
            };

            var validationResult = new FindProductDtoValidator().Validate(dto);

            if (!validationResult.IsValid)
                return ValidationUtils.GenerateValidationError(log, validationResult);

            var productExists = await ProductsRepository.Delete(dto);

            if (!productExists)
            {
                log.LogInformation($"Product with id: {id} and companyId: {companyId} not found");
                return new NotFoundResult();
            }

            log.LogInformation($"Product with id: {id} and companyId: {companyId} deleted successfully");
            return new OkObjectResult(productExists);
        }
    }
}
