using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using ProductsFunctionApp.Models;
using ProductsFunctionApp.Validation;
using ProductsFunctionApp.Dto;
using ProductsFunctionApp.Repository;
using ProductsFunctionApp.Utils;

namespace ProductsFunctionApp.Functions
{
    public static class CreateProduct
    {
        [FunctionName("CreateProduct")]
        public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "products")] HttpRequest req,
        ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            log.LogInformation($"Create product function called with data: {requestBody}");
            var dto = JsonConvert.DeserializeObject<CreateProductDto>(requestBody);
            var validationResult = new CreateProductDtoValidator().Validate(dto);

            if (!validationResult.IsValid)
                return ValidationUtils.GenerateValidationError(log, validationResult);

            var id = await ProductsRepository.Add(dto);
            var product = new Product()
            {
                BrandName = dto.BrandName,
                Name = dto.Name,
                CompanyId = dto.CompanyId,
                Id = id
            };

            log.LogInformation($"Product with id: {id} created successfully");
            return new OkObjectResult(product);
        }
    }
}
