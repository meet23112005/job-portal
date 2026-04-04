using FluentValidation;
using System.Net;
using System.Text.Json;

namespace Job_portal.API.Middleware;

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);//run next middleware or controller
            }
            catch (ValidationException ex)
            {
                // FluentValidation failed
                // ValidationBehavior threw this
                _logger.LogWarning("Validation Faild: {Errors}",
                    string.Join(", ",ex.Errors.Select(e => e.ErrorMessage)));

                await WriteErrorResponse(context,
                    HttpStatusCode.BadRequest,
                    "Validation Failed.",
                    ex.Errors.Select(e => e.ErrorMessage));
            }
            catch(UnauthorizedAccessException ex)
            {
                _logger.LogWarning("UnAuthorized : {Message}",ex.Message);

                await WriteErrorResponse(context,
                    HttpStatusCode.Unauthorized,
                    ex.Message);
            }
            catch(KeyNotFoundException ex)
            {
                _logger.LogWarning($"Not Found {ex.Message}");

                await WriteErrorResponse(context,
                    HttpStatusCode.NotFound, 
                    ex.Message);
            }catch(Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occured.");

                await WriteErrorResponse(
                    context,
                    HttpStatusCode.InternalServerError,
                    "Something went wrong. Please try again.");
            }
        }

        private async Task WriteErrorResponse(
            HttpContext context,
            HttpStatusCode statusCode,
            string Message,
            IEnumerable<string>? errors = null)//optional — only validation has errors list
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                success = false,
                message = Message,
                errors = errors ?? Enumerable.Empty<string>()
            };
            var json = JsonSerializer.Serialize(response,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    //converts C# PascalCase to JavaScript camelCase
                });

            await context.Response.WriteAsync(json);    
        }
    }

//ValidationException
//→ thrown by ValidationBehavior
//→ FluentValidation rules failed
//→ returns 400 Bad Request
//→ includes all error messages
//→ frontend shows field errors

//UnauthorizedAccessException
//→ thrown manually in handlers
//→ example: recruiter tries to edit someone else's job
//→ returns 401 Unauthorized

//KeyNotFoundException
//→ thrown when resource not found
//→ optional — we currently return false from handlers
//→ good to have for future use
//→ returns 404 Not Found