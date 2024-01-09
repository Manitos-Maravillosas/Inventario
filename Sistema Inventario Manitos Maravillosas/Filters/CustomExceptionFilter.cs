using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Sistema_Inventario_Manitos_Maravillosas.Models;

namespace Sistema_Inventario_Manitos_Maravillosas.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly IFileLogger _logger;

        public CustomExceptionFilter(IFileLogger logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is CustomDataException)
            {
                // Log the exception
                _logger.LogError(context.Exception);

                // Redirect to the error view
                context.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/Error.cshtml" // Adjust the path as needed
                };
            }
        }
    }

}
