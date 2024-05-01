using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ProductsFunctionApp.Repository;
using ProductsFunctionApp.Validation;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using ProductsFunctionApp.Utils;
using ProductsFunctionApp.Models;

namespace ProductsFunctionApp.Functions
{
    public static class UpdateProduct
    {
        [FunctionName("UpdateProduct")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "products")] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            log.LogInformation($"Update product function called with data: {requestBody}");
            var product = JsonConvert.DeserializeObject<Product>(requestBody);
            var validationResult = new ProductValidator().Validate(product);

            if (!validationResult.IsValid)
                return ValidationUtils.GenerateValidationError(log, validationResult);

            var productExists = await ProductsRepository.Update(product);

            if (!productExists)
            {
                log.LogInformation($"Product with id: {product.Id} and companyId: {product.CompanyId} not found");
                return new NotFoundResult();
            }

            log.LogInformation($"Product with id: {product.Id} and companyId: {product.CompanyId} deleted successfully");
            return new OkObjectResult(productExists);
        }
    }
}
