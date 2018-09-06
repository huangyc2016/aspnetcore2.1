using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HYC.WebApi.swagger
{
    public class HiddenFilter : IDocumentFilter
    {
        private readonly IConfiguration _configuration;

        public HiddenFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            if (_configuration["RunEnvironment"]== "product")
                swaggerDoc.Paths.Clear();
        }
    }
}
