using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using OLAP.API.Infrastructure.Exceptions;
using OLAP.API.Models.Response.Error;
using FluentValidation;

namespace OLAP.API.Infrastructure.Filters
{
    internal class CommandValidationExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is CommandValidationException ex)
            {
                var responseData = new JsonErrorResponse()
                {
                    DeveloperMessage = ex.Message
                };
                var result = new ContentResult
                {
                    ContentType = "application/json",
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                if (ex.Args?.Any() ?? false)
                {
                    responseData.Arguments = ex.Args?.Select(x => x.Key + ": " + x.Value).ToList();
                }
                if (ex.InnerException is ValidationException validationException)
                {
                    responseData.ClientMessage = validationException.Errors?.FirstOrDefault()?.ErrorMessage;
                    responseData.Errors = validationException.Errors?.Select(x => x.ErrorMessage).ToList();
                }
                result.Content = JsonConvert.SerializeObject(responseData);
                context.Result = result;
                context.ExceptionHandled = true;
            }

            base.OnException(context);
        }
    }
}
