using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IFRS16_Backend.Swagger
{
    public class SwaggerFileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // collect file properties if present on parameters or their properties
            var fileProps = new Dictionary<string, OpenApiSchema>();
            var required = new HashSet<string>();

            foreach (var param in context.MethodInfo.GetParameters())
            {
                var pType = param.ParameterType;

                if (pType == typeof(IFormFile) || pType == typeof(IEnumerable<IFormFile>))
                {
                    fileProps["file"] = new OpenApiSchema { Type = "string", Format = "binary" };
                    required.Add("file");
                }
                else
                {
                    var props = pType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (var prop in props)
                    {
                        if (prop.PropertyType == typeof(IFormFile) || prop.PropertyType == typeof(IEnumerable<IFormFile>))
                        {
                            fileProps[prop.Name] = new OpenApiSchema { Type = "string", Format = "binary" };
                            required.Add(prop.Name);
                        }
                    }
                }
            }

            // If no file props found, but the action supports multipart/form-data, add a generic file field
            bool supportsMultipart = context.MethodInfo.GetCustomAttributes(true)
                .OfType<ConsumesAttribute>()
                .Any(a => a.ContentTypes.Any(ct => ct.Contains("multipart/form-data")))
                || context.ApiDescription.SupportedRequestFormats.Any(r => r.MediaType?.Contains("multipart/form-data") == true);

            if (!fileProps.Any() && supportsMultipart)
            {
                fileProps["file"] = new OpenApiSchema { Type = "string", Format = "binary" };
                required.Add("file");
            }

            if (fileProps.Any())
            {
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["multipart/form-data"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties = fileProps.ToDictionary(k => k.Key, v => v.Value),
                                Required = new SortedSet<string>(required)
                            }
                        }
                    }
                };
            }
        }
    }
}
