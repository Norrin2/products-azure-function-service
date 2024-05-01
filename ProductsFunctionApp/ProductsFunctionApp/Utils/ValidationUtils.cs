using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace ProductsFunctionApp.Utils
{
    public static class ValidationUtils
    {
        public static BadRequestObjectResult GenerateValidationError(ILogger log, ValidationResult validationResult)
        {
            var validationErrors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            log.LogError($"Invalid input parameters: {validationErrors}");
            return new BadRequestObjectResult(validationErrors);
        }
    }
}
