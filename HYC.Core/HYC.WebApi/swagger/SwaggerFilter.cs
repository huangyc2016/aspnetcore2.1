
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HYC.WebApi.swagger
{
    public class SwaggerFilter: IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var controllerActionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

            if (operation.Parameters != null && operation.Parameters.Count > 0)
            {
                //参数过滤实体类中不必要的属性
                var paramIgnoreNames = context.ApiDescription.ActionDescriptor.Parameters.SelectMany(x => x.ParameterType.GetProperties())
                    .Where(x => x.CustomAttributes.Any(p => p.AttributeType == typeof(SwaggerParamIgnore)))
                    .Select(x => x.Name).ToList();
                if (paramIgnoreNames.Count > 0)
                {
                    foreach (var name in paramIgnoreNames)
                    {
                        var paramItem = operation.Parameters.FirstOrDefault(p => p.Name == name);
                        if (paramItem != null)
                            operation.Parameters.Remove(paramItem);
                    }
                }

                //给不为空的参数加上属性
                var requiredParams = context.ApiDescription.ParameterDescriptions.Where(x => x.ModelMetadata.IsRequired == true).Select(x => x.Name).ToList();
                //自定义不能为只的属性
                var customRequiredAttr = controllerActionDescriptor.MethodInfo.GetCustomAttribute<RequiredParams>();
                if (customRequiredAttr != null)
                {
                    if (!string.IsNullOrWhiteSpace(customRequiredAttr.ParmaArr))
                    {
                        requiredParams.AddRange(customRequiredAttr.ParmaArr.Split(','));
                    }
                }
                if (requiredParams.Count > 0)
                {
                    foreach (var name in requiredParams)
                    {
                        if (operation.Parameters.Any(p => p.Name == name))
                            operation.Parameters.First(p => p.Name == name).Required = true;
                    }
                }
            }
        }
    }

    public class SwaggerParamIgnore : Attribute
    {

    }

    public class RequiredParams : Attribute
    {
        public string ParmaArr = "";

        public RequiredParams(string paramArr)
        {
            ParmaArr = paramArr;
        }
    }
}

