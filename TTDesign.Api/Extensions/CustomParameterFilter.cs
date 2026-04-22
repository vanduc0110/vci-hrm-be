using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using TTDesign.API.Extensions.Swagger;

namespace TTDesign.API.Extensions
{
  public class CustomParameterFilter : IParameterFilter
  {
    public void Apply( OpenApiParameter parameter, ParameterFilterContext context )
    {
      IEnumerable<SwaggerParameterExampleAttribute>? parameterAttributes = null;

      if ( context.PropertyInfo != null ) {
        parameterAttributes = context.PropertyInfo.GetCustomAttributes<SwaggerParameterExampleAttribute>();
      }
      else if ( context.ParameterInfo != null ) {
        parameterAttributes = context.ParameterInfo.GetCustomAttributes<SwaggerParameterExampleAttribute>();
      }
      if ( parameterAttributes != null && parameterAttributes.Any() ) {
        AddExample( parameter, parameterAttributes );
      }
    }

    private void AddExample( OpenApiParameter parameter, IEnumerable<SwaggerParameterExampleAttribute> parameterAttributes )
    {
      foreach ( var attribute in parameterAttributes ) {
        var example = new OpenApiExample
        {
          Value = new Microsoft.OpenApi.Any.OpenApiString( attribute.Value )
        };
        parameter.Examples.Add( attribute.Name, example );
      }
    }
  }
}
