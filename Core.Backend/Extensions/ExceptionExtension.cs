
using Core.CoreLib.Models.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Core.Backend.Extensions
{
    public static class ExceptionExtension
    {
        public static string Detail(this Exception exception, ILogger? logger = null)
        {
            try
            {
                var exceptionDetail =
                    $"{exception.Message} \r\n" +
                    $"{(exception.InnerException != null ? "Inner Exception: " + exception.InnerException.Message + "\r\n" : string.Empty)} " +
                    $"{(exception.InnerException?.InnerException != null ? "Inner Inner Exception: " + exception.InnerException.InnerException.Message + "\r\n" : string.Empty)} " +
                    $"Stack: {exception.StackTrace}";

                // Log
                if (logger != null)
                    logger.LogError(exception, $"Logger: {exceptionDetail}");

                return exceptionDetail;
            }
            catch (Exception ex)
            {
                return
                    $"Error in exception detail. Original exception: {ex.Message} Unexpected exception: {ex.Message} Stack: {ex.StackTrace}";
            }
        }

        public static ObjectResult PackageExceptionResponse(this Exception ex, ILogger? logger = null) =>
            new ObjectResult(ex.Detail(logger)) 
            { 
                StatusCode =
                    ex.GetType() == typeof(DataNotFoundException) ?
                        (int)HttpStatusCode.NotFound :
                        ex.GetType() == typeof(BadRequestException) ?
                            (int)HttpStatusCode.BadRequest :
                            ex.GetType() == typeof(NotImplementedException) ?
                                (int)HttpStatusCode.NotImplemented :
                                // InternalServerError should always be last default
                                (int)HttpStatusCode.InternalServerError 
            };
    }
}